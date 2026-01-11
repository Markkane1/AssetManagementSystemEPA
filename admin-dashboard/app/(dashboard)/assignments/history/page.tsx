"use client";

import { useState } from "react";
import { useQuery } from "@tanstack/react-query";

import { apiFetch } from "@/src/api/client";
import { Card, CardContent, CardHeader, CardTitle } from "@/src/components/ui/card";
import { Button } from "@/src/components/ui/button";
import { Input } from "@/src/components/ui/input";
import { DataTable } from "@/src/components/data-table";

export default function AssignmentHistoryPage() {
  const [mode, setMode] = useState<"employee" | "asset">("employee");
  const [id, setId] = useState("");

  const query = useQuery({
    queryKey: ["assignments", "history", mode, id],
    queryFn: () =>
      apiFetch<any[]>(
        mode === "employee"
          ? `/api/Assignments/history/employee/${id}`
          : `/api/Assignments/history/asset/${id}`,
        { method: "GET" }
      ),
    enabled: Boolean(id)
  });

  return (
    <Card>
      <CardHeader>
        <CardTitle>Assignment History</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <div className="flex flex-wrap gap-2">
          <Button variant={mode === "employee" ? "default" : "outline"} onClick={() => setMode("employee")}>
            By Employee
          </Button>
          <Button variant={mode === "asset" ? "default" : "outline"} onClick={() => setMode("asset")}>
            By Asset Item
          </Button>
        </div>
        <div className="flex gap-2">
          <Input
            placeholder={mode === "employee" ? "Employee ID" : "Asset Item ID"}
            value={id}
            onChange={(event) => setId(event.target.value)}
          />
        </div>
        {query.isLoading ? (
          <p className="text-sm text-muted-foreground">Loading history...</p>
        ) : query.isError ? (
          <p className="text-sm text-destructive">Failed to load history.</p>
        ) : query.data ? (
          <DataTable data={query.data} />
        ) : (
          <p className="text-sm text-muted-foreground">Enter an ID to fetch history.</p>
        )}
      </CardContent>
    </Card>
  );
}
