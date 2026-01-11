"use client";

import { useQuery } from "@tanstack/react-query";
import { Bar, BarChart, ResponsiveContainer, Tooltip, XAxis, YAxis, Pie, PieChart } from "recharts";

import { apiFetch } from "@/src/api/client";
import { Card, CardContent, CardHeader, CardTitle } from "@/src/components/ui/card";

export default function DashboardPage() {
  const assetSummaryQuery = useQuery({
    queryKey: ["reports", "asset-summary-location"],
    queryFn: () => apiFetch<any[]>("/api/Reports/asset-summary/location", { method: "GET" })
  });

  const categorySummaryQuery = useQuery({
    queryKey: ["reports", "asset-summary-category"],
    queryFn: () => apiFetch<any[]>("/api/Reports/asset-summary/category", { method: "GET" })
  });

  const statusSummaryQuery = useQuery({
    queryKey: ["reports", "asset-status"],
    queryFn: () => apiFetch<any[]>("/api/Reports/asset-status", { method: "GET" })
  });

  const recentAssignmentsQuery = useQuery({
    queryKey: ["assignments", "recent"],
    queryFn: () => apiFetch<any[]>("/api/Assignments", { method: "GET" })
  });

  const assetSummary = assetSummaryQuery.data ?? [];
  const categorySummary = categorySummaryQuery.data ?? [];
  const statusSummary = statusSummaryQuery.data ?? [];
  const recentAssignments = recentAssignmentsQuery.data ?? [];

  return (
    <div className="space-y-6">
      <div className="grid gap-4 md:grid-cols-3">
        <Card>
          <CardHeader>
            <CardTitle>Total Assets</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-3xl font-semibold">
              {assetSummary.reduce((acc, item) => acc + (item?.totalAssets ?? 0), 0)}
            </p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader>
            <CardTitle>Categories</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-3xl font-semibold">{categorySummary.length}</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader>
            <CardTitle>Assignments</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-3xl font-semibold">{recentAssignments.length}</p>
          </CardContent>
        </Card>
      </div>

      <div className="grid gap-6 lg:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>Assets by Location</CardTitle>
          </CardHeader>
          <CardContent className="h-72">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={assetSummary}>
                <XAxis dataKey="locationName" />
                <YAxis />
                <Tooltip />
                <Bar dataKey="totalAssets" fill="#2563eb" />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
        <Card>
          <CardHeader>
            <CardTitle>Assets by Status</CardTitle>
          </CardHeader>
          <CardContent className="h-72">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie data={statusSummary} dataKey="count" nameKey="status" fill="#60a5fa" label />
                <Tooltip />
              </PieChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Recent Activity</CardTitle>
        </CardHeader>
        <CardContent>
          {recentAssignments.length ? (
            <ul className="space-y-2 text-sm">
              {recentAssignments.slice(0, 6).map((assignment, index) => (
                <li key={index} className="rounded-md border p-3">
                  Assignment #{assignment?.id ?? "-"} — Asset Item {assignment?.assetItemId ?? "-"}
                </li>
              ))}
            </ul>
          ) : (
            <p className="text-sm text-muted-foreground">
              No recent activity yet. Once assignments are created they will appear here.
            </p>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
