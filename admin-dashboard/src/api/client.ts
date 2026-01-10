import { useAuthStore } from "@/src/auth/store";
import type { components } from "@/src/api/types";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL;

export type ApiError = {
  status: number;
  message: string;
  details?: unknown;
};

async function parseError(response: Response): Promise<ApiError> {
  let details: unknown = undefined;
  try {
    details = await response.json();
  } catch {
    details = await response.text();
  }
  return {
    status: response.status,
    message: response.statusText,
    details
  };
}

async function refreshSession() {
  const { token, refreshToken } = useAuthStore.getState();
  if (!token || !refreshToken) return null;

  const body: components["schemas"]["RefreshTokenRequest"] = {
    accessToken: token,
    refreshToken
  };

  const response = await fetch(`${API_BASE_URL}/api/Auth/refresh-token`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(body)
  });

  if (!response.ok) {
    return null;
  }

  const data = (await response.json()) as components["schemas"]["AuthResponseDto"];
  useAuthStore.getState().setSession({
    token: data.token ?? null,
    refreshToken: data.refreshToken ?? null,
    expiresAt: data.expiresAt ?? null,
    user: data.user ?? null
  });

  return data;
}

export async function apiFetch<T>(
  input: string,
  init?: RequestInit,
  options?: { retryOnUnauthorized?: boolean }
): Promise<T> {
  if (!API_BASE_URL) {
    throw new Error("NEXT_PUBLIC_API_BASE_URL is not configured");
  }

  const token = useAuthStore.getState().token;
  const headers = new Headers(init?.headers || {});
  headers.set("Content-Type", "application/json");
  if (token) {
    headers.set("Authorization", `Bearer ${token}`);
  }

  const response = await fetch(`${API_BASE_URL}${input}`, {
    ...init,
    headers
  });

  if (response.status === 401 && options?.retryOnUnauthorized !== false) {
    const refreshed = await refreshSession();
    if (refreshed?.token) {
      return apiFetch<T>(input, init, { retryOnUnauthorized: false });
    }
  }

  if (!response.ok) {
    throw await parseError(response);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  const contentType = response.headers.get("content-type");
  if (contentType?.includes("application/json")) {
    return (await response.json()) as T;
  }

  return (await response.text()) as T;
}
