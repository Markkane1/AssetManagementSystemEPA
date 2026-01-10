"use client";

import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { Bar, BarChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from "recharts";

import { reportConfigs } from "@/src/features/reports/report-config";
import { apiFetch } from "@/src/api/client";
import { Card, CardContent, CardHeader, CardTitle } from "@/src/components/ui/card";
import { DataTable } from "@/src/components/data-table";
import { Button } from "@/src/components/ui/button";

function exportCsv(data: Record<string, unknown>[], filename: string) {
  if (!data.length) return;
  const headers = Object.keys(data[0]);
  const csv = [headers.join(",")]
    .concat(
      data.map((row) =>
        headers
          .map((header) => JSON.stringify(row[header] ?? ""))
          .join(",")
      )
    )
    .join("\n");
  const blob = new Blob([csv], { type: "text/csv" });
  const url = URL.createObjectURL(blob);
  const link = document.createElement("a");
  link.href = url;
  link.download = `${filename}.csv`;
  link.click();
  URL.revokeObjectURL(url);
}

export default function ReportsPage() {
  const [activeReport, setActiveReport] = useState(reportConfigs[0]);
  const query = useQuery({
    queryKey: ["reports", activeReport.key],
    queryFn: () => apiFetch<any[]>(activeReport.endpoint, { method: "GET" })
  });

  return (
    <div className="grid gap-6 lg:grid-cols-[300px_1fr]">
      <Card>
        <CardHeader>
          <CardTitle>Reports</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-2">
            {reportConfigs.map((report) => (
              <Button
                key={report.key}
                variant={report.key === activeReport.key ? "default" : "outline"}
                className="w-full justify-start"
                onClick={() => setActiveReport(report)}
              >
                {report.label}
              </Button>
            ))}
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader className="flex flex-row items-center justify-between">
          <CardTitle>{activeReport.label}</CardTitle>
          <Button
            variant="outline"
            onClick={() => exportCsv(query.data ?? [], activeReport.key)}
          >
            Export CSV
          </Button>
        </CardHeader>
        <CardContent className="space-y-6">
          {query.isLoading ? (
            <p className="text-sm text-muted-foreground">Loading report...</p>
          ) : query.isError ? (
            <p className="text-sm text-destructive">Failed to load report.</p>
          ) : (
            <>
              <div className="h-72">
                <ResponsiveContainer width="100%" height="100%">
                  <BarChart data={query.data ?? []}>
                    <XAxis dataKey={Object.keys(query.data?.[0] ?? {})[0]} />
                    <YAxis />
                    <Tooltip />
                    <Bar dataKey={Object.keys(query.data?.[0] ?? {})[1]} fill="#2563eb" />
                  </BarChart>
                </ResponsiveContainer>
              </div>
              <DataTable data={query.data ?? []} />
            </>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
