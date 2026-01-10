"use client";

import { useState } from "react";
import { useQuery } from "@tanstack/react-query";

import { apiFetch } from "@/src/api/client";
import { Card, CardContent, CardHeader, CardTitle } from "@/src/components/ui/card";
import { DataTable } from "@/src/components/data-table";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/src/components/ui/select";

export default function PermissionsPage() {
  const [category, setCategory] = useState<string | null>(null);

  const categoriesQuery = useQuery({
    queryKey: ["permissions", "categories"],
    queryFn: () => apiFetch<any[]>("/api/Permissions/categories", { method: "GET" })
  });

  const permissionsQuery = useQuery({
    queryKey: ["permissions", category ?? "all"],
    queryFn: () =>
      category
        ? apiFetch<any[]>(`/api/Permissions/category/${category}`, { method: "GET" })
        : apiFetch<any[]>("/api/Permissions", { method: "GET" })
  });

  return (
    <Card>
      <CardHeader>
        <CardTitle>Permissions</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <div className="max-w-sm">
          <Select onValueChange={setCategory}>
            <SelectTrigger>
              <SelectValue placeholder="Filter by category" />
            </SelectTrigger>
            <SelectContent>
              {(categoriesQuery.data ?? []).map((item) => (
                <SelectItem key={String(item)} value={String(item)}>
                  {String(item)}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>
        {permissionsQuery.isLoading ? (
          <p className="text-sm text-muted-foreground">Loading permissions...</p>
        ) : (
          <DataTable data={permissionsQuery.data ?? []} />
        )}
      </CardContent>
    </Card>
  );
}
