"use client";

import { useMutation } from "@tanstack/react-query";

import { apiFetch } from "@/src/api/client";
import { AutoForm } from "@/src/components/forms/auto-form";
import { Card, CardContent, CardHeader, CardTitle } from "@/src/components/ui/card";
import { toast } from "@/src/components/ui/use-toast";

export default function ReturnAssignmentPage() {
  const mutation = useMutation({
    mutationFn: (values: Record<string, unknown>) =>
      apiFetch("/api/Assignments/return", {
        method: "POST",
        body: JSON.stringify(values)
      }),
    onSuccess: () => toast({ title: "Asset returned" }),
    onError: () => toast({ title: "Return failed", variant: "destructive" })
  });

  return (
    <Card>
      <CardHeader>
        <CardTitle>Return Asset Item</CardTitle>
      </CardHeader>
      <CardContent>
        <AutoForm
          schemaName="ReturnAssetItemRequest"
          onSubmit={(values) => mutation.mutate(values)}
          submitLabel="Return"
        />
      </CardContent>
    </Card>
  );
}
