"use client";

import { useQuery } from "@tanstack/react-query";

import { apiFetch } from "@/src/api/client";
import { Card, CardContent, CardHeader, CardTitle } from "@/src/components/ui/card";
import { DataTable } from "@/src/components/data-table";

export default function OverdueAssignmentsPage() {
  const query = useQuery({
    queryKey: ["assignments", "overdue"],
    queryFn: () => apiFetch<any[]>("/api/Assignments/overdue", { method: "GET" })
  });

  return (
    <Card>
      <CardHeader>
        <CardTitle>Overdue Assignments</CardTitle>
      </CardHeader>
      <CardContent>
        {query.isLoading ? (
          <p className="text-sm text-muted-foreground">Loading overdue assignments...</p>
        ) : query.isError ? (
          <p className="text-sm text-destructive">Failed to load overdue assignments.</p>
        ) : (
          <DataTable data={query.data ?? []} />
        )}
      </CardContent>
    </Card>
  );
}
