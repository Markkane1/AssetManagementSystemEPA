"use client";

import { useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import { apiFetch } from "@/src/api/client";
import { Card, CardContent, CardHeader, CardTitle } from "@/src/components/ui/card";
import { Button } from "@/src/components/ui/button";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/src/components/ui/select";
import { toast } from "@/src/components/ui/use-toast";

export default function RolesPage() {
  const queryClient = useQueryClient();
  const [selectedRole, setSelectedRole] = useState<string | null>(null);
  const [selectedPermission, setSelectedPermission] = useState<string | null>(null);

  const rolesQuery = useQuery({
    queryKey: ["roles"],
    queryFn: () => apiFetch<any[]>("/api/roles", { method: "GET" })
  });

  const permissionsQuery = useQuery({
    queryKey: ["permissions"],
    queryFn: () => apiFetch<any[]>("/api/Permissions", { method: "GET" })
  });

  const rolePermissionsQuery = useQuery({
    queryKey: ["roles", selectedRole, "permissions"],
    queryFn: () => apiFetch<any[]>(`/api/roles/${selectedRole}/permissions`, { method: "GET" }),
    enabled: Boolean(selectedRole)
  });

  const assignMutation = useMutation({
    mutationFn: () =>
      apiFetch(`/api/roles/${selectedRole}/permissions`, {
        method: "POST",
        body: JSON.stringify({ permissionId: Number(selectedPermission) })
      }),
    onSuccess: () => {
      toast({ title: "Permission assigned" });
      queryClient.invalidateQueries({ queryKey: ["roles", selectedRole, "permissions"] });
    },
    onError: () => toast({ title: "Assignment failed", variant: "destructive" })
  });

  const removeMutation = useMutation({
    mutationFn: (permissionId: number) =>
      apiFetch(`/api/roles/${selectedRole}/permissions/${permissionId}`, { method: "DELETE" }),
    onSuccess: () => {
      toast({ title: "Permission removed" });
      queryClient.invalidateQueries({ queryKey: ["roles", selectedRole, "permissions"] });
    },
    onError: () => toast({ title: "Removal failed", variant: "destructive" })
  });

  return (
    <div className="grid gap-6 lg:grid-cols-2">
      <Card>
        <CardHeader>
          <CardTitle>Roles</CardTitle>
        </CardHeader>
        <CardContent>
          {rolesQuery.isLoading ? (
            <p className="text-sm text-muted-foreground">Loading roles...</p>
          ) : (
            <div className="space-y-2">
              {(rolesQuery.data ?? []).map((role) => (
                <Button
                  key={role?.id ?? role?.name}
                  variant={selectedRole === role?.id ? "default" : "outline"}
                  className="w-full justify-start"
                  onClick={() => setSelectedRole(role?.id ?? role?.name)}
                >
                  {role?.name ?? role?.id}
                </Button>
              ))}
            </div>
          )}
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Role Permissions</CardTitle>
        </CardHeader>
        <CardContent className="space-y-4">
          {!selectedRole ? (
            <p className="text-sm text-muted-foreground">Select a role to view permissions.</p>
          ) : (
            <>
              <div className="flex gap-2">
                <Select onValueChange={setSelectedPermission}>
                  <SelectTrigger>
                    <SelectValue placeholder="Select permission" />
                  </SelectTrigger>
                  <SelectContent>
                    {(permissionsQuery.data ?? []).map((permission) => (
                      <SelectItem
                        key={permission?.id ?? permission?.name}
                        value={String(permission?.id ?? "")}
                      >
                        {permission?.name ?? permission?.id}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
                <Button
                  onClick={() => assignMutation.mutate()}
                  disabled={!selectedPermission || assignMutation.isPending}
                >
                  Assign
                </Button>
              </div>
              <div className="space-y-2">
                {(rolePermissionsQuery.data ?? []).map((permission) => (
                  <div
                    key={permission?.id ?? permission?.name}
                    className="flex items-center justify-between rounded-md border p-2"
                  >
                    <span>{permission?.name ?? permission?.id}</span>
                    <Button
                      variant="destructive"
                      size="sm"
                      onClick={() => removeMutation.mutate(Number(permission?.id))}
                    >
                      Remove
                    </Button>
                  </div>
                ))}
              </div>
            </>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
