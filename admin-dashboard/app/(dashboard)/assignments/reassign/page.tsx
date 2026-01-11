"use client";

import { useMutation } from "@tanstack/react-query";

import { apiFetch } from "@/src/api/client";
import { AutoForm } from "@/src/components/forms/auto-form";
import { Card, CardContent, CardHeader, CardTitle } from "@/src/components/ui/card";
import { toast } from "@/src/components/ui/use-toast";

export default function ReassignAssignmentPage() {
  const mutation = useMutation({
    mutationFn: (values: Record<string, unknown>) =>
      apiFetch("/api/Assignments/reassign", {
        method: "POST",
        body: JSON.stringify(values)
      }),
    onSuccess: () => toast({ title: "Asset reassigned" }),
    onError: () => toast({ title: "Reassign failed", variant: "destructive" })
  });

  return (
    <Card>
      <CardHeader>
        <CardTitle>Reassign Asset Item</CardTitle>
      </CardHeader>
      <CardContent>
        <AutoForm
          schemaName="ReassignAssetItemRequest"
          onSubmit={(values) => mutation.mutate(values)}
          submitLabel="Reassign"
        />
      </CardContent>
    </Card>
  );
}
