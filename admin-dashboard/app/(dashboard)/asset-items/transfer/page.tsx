"use client";

import { useMutation } from "@tanstack/react-query";

import { apiFetch } from "@/src/api/client";
import { AutoForm } from "@/src/components/forms/auto-form";
import { Card, CardContent, CardHeader, CardTitle } from "@/src/components/ui/card";
import { toast } from "@/src/components/ui/use-toast";

export default function TransferAssetsPage() {
  const mutation = useMutation({
    mutationFn: (values: Record<string, unknown>) =>
      apiFetch("/api/AssetItems/transfer", {
        method: "POST",
        body: JSON.stringify(values)
      }),
    onSuccess: () => toast({ title: "Assets transferred" }),
    onError: () => toast({ title: "Transfer failed", variant: "destructive" })
  });

  return (
    <Card>
      <CardHeader>
        <CardTitle>Transfer Asset Items</CardTitle>
      </CardHeader>
      <CardContent>
        <AutoForm
          schemaName="TransferAssetsRequest"
          onSubmit={(values) => mutation.mutate(values)}
          submitLabel="Transfer"
        />
      </CardContent>
    </Card>
  );
}
