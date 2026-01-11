import type { components } from "@/src/api/types";
import { apiFetch } from "@/src/api/client";

export type ResourceConfig = {
  key: string;
  label: string;
  basePath: string;
  idParam?: string;
  createSchema?: keyof components["schemas"];
  updateSchema?: keyof components["schemas"];
  detailSchema?: keyof components["schemas"];
  permissions?: {
    view?: string;
    create?: string;
    edit?: string;
    delete?: string;
  };
};

export async function listResource<T>(config: ResourceConfig) {
  return apiFetch<T>(config.basePath, { method: "GET" });
}

export async function getResource<T>(config: ResourceConfig, id: string) {
  return apiFetch<T>(`${config.basePath}/${id}`, { method: "GET" });
}

export async function createResource<T>(config: ResourceConfig, data: unknown) {
  return apiFetch<T>(config.basePath, {
    method: "POST",
    body: JSON.stringify(data)
  });
}

export async function updateResource<T>(config: ResourceConfig, data: unknown) {
  return apiFetch<T>(config.basePath, {
    method: "PUT",
    body: JSON.stringify(data)
  });
}

export async function deleteResource<T>(config: ResourceConfig, id: string) {
  return apiFetch<T>(`${config.basePath}/${id}`, { method: "DELETE" });
}
