"use client";

import { useState } from "react";
import { useMutation, useQuery } from "@tanstack/react-query";

import { apiFetch } from "@/src/api/client";
import { Card, CardContent, CardHeader, CardTitle } from "@/src/components/ui/card";
import { Input } from "@/src/components/ui/input";
import { Button } from "@/src/components/ui/button";
import { DataTable } from "@/src/components/data-table";
import { toast } from "@/src/components/ui/use-toast";

export default function UserAccessPage() {
  const [userId, setUserId] = useState("");
  const [locationId, setLocationId] = useState("");

  const accessQuery = useQuery({
    queryKey: ["user-access", userId],
    queryFn: () => apiFetch<any[]>(`/api/users/${userId}/access/locations`, { method: "GET" }),
    enabled: Boolean(userId)
  });

  const assignMutation = useMutation({
    mutationFn: () =>
      apiFetch(`/api/users/${userId}/access/locations`, {
        method: "POST",
        body: JSON.stringify({ locationId })
      }),
    onSuccess: () => {
      toast({ title: "Access granted" });
      accessQuery.refetch();
    },
    onError: () => toast({ title: "Grant failed", variant: "destructive" })
  });

  return (
    <Card>
      <CardHeader>
        <CardTitle>User Access</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <div className="grid gap-4 md:grid-cols-2">
          <div className="space-y-2">
            <label className="text-sm font-medium">User ID</label>
            <Input value={userId} onChange={(event) => setUserId(event.target.value)} />
          </div>
          <div className="space-y-2">
            <label className="text-sm font-medium">Location ID</label>
            <Input value={locationId} onChange={(event) => setLocationId(event.target.value)} />
          </div>
        </div>
        <Button
          onClick={() => assignMutation.mutate()}
          disabled={!userId || !locationId || assignMutation.isPending}
        >
          Grant Access
        </Button>
        {accessQuery.isLoading ? (
          <p className="text-sm text-muted-foreground">Loading access...</p>
        ) : accessQuery.data ? (
          <DataTable data={accessQuery.data} />
        ) : (
          <p className="text-sm text-muted-foreground">Enter a user ID to view access.</p>
        )}
      </CardContent>
    </Card>
  );
}
