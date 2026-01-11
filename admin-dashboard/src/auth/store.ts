import { create } from "zustand";
import { persist } from "zustand/middleware";

import type { components } from "@/src/api/types";

export type UserInfo = components["schemas"]["UserInfoDto"];

type AuthState = {
  token: string | null;
  refreshToken: string | null;
  expiresAt: string | null;
  user: UserInfo | null;
  setSession: (payload: {
    token?: string | null;
    refreshToken?: string | null;
    expiresAt?: string | null;
    user?: UserInfo | null;
  }) => void;
  clearSession: () => void;
  can: (permission: string) => boolean;
};

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      token: null,
      refreshToken: null,
      expiresAt: null,
      user: null,
      setSession: (payload) =>
        set((state) => ({
          token: payload.token ?? state.token,
          refreshToken: payload.refreshToken ?? state.refreshToken,
          expiresAt: payload.expiresAt ?? state.expiresAt,
          user: payload.user ?? state.user
        })),
      clearSession: () =>
        set({ token: null, refreshToken: null, expiresAt: null, user: null }),
      can: (permission: string) => {
        const user = get().user;
        if (!user?.permissions) return false;
        return user.permissions.includes(permission);
      }
    }),
    {
      name: "asset-management-auth"
    }
  )
);
