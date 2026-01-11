"use client";

import * as React from "react";

import { useAuthStore } from "@/src/auth/store";

export function RequirePermission({
  permission,
  children
}: {
  permission: string;
  children: React.ReactNode;
}) {
  const can = useAuthStore((state) => state.can);
  if (!can(permission)) return null;
  return <>{children}</>;
}
