"use client";

import { useRouter } from "next/navigation";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";

import {
  listResource,
  getResource,
  createResource,
  updateResource,
  deleteResource,
  type ResourceConfig
} from "@/src/api/resources";
import { AutoForm } from "@/src/components/forms/auto-form";
import { DataTable } from "@/src/components/data-table";
import { Button } from "@/src/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/src/components/ui/card";
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from "@/src/components/ui/dialog";
import { toast } from "@/src/components/ui/use-toast";
import { RequirePermission } from "@/src/auth/require-permission";
import { useState } from "react";

export function ResourceListPage({ config }: { config: ResourceConfig }) {
  const router = useRouter();
  const query = useQuery({
    queryKey: [config.key, "list"],
    queryFn: () => listResource<any[]>(config)
  });

  return (
    <Card>
      <CardHeader className="flex flex-row items-center justify-between">
        <CardTitle>{config.label}</CardTitle>
        <RequirePermission permission={config.permissions?.create ?? ""}>
          <Button onClick={() => router.push(`/${config.key}/create`)}>New {config.label}</Button>
        </RequirePermission>
      </CardHeader>
      <CardContent>
        {query.isLoading ? (
          <p className="text-sm text-muted-foreground">Loading...</p>
        ) : query.isError ? (
          <p className="text-sm text-destructive">Failed to load data.</p>
        ) : (
          <DataTable
            data={query.data ?? []}
            onRowClick={(row) => {
              const id = (row as any)?.id ?? (row as any)?.assetId ?? (row as any)?.employeeId;
              if (id) {
                router.push(`/${config.key}/${id}`);
              }
            }}
          />
        )}
      </CardContent>
    </Card>
  );
}

export function ResourceDetailPage({
  config,
  id
}: {
  config: ResourceConfig;
  id: string;
}) {
  const router = useRouter();
  const queryClient = useQueryClient();
  const [open, setOpen] = useState(false);
  const deleteMutation = useMutation({
    mutationFn: () => deleteResource(config, id),
    onSuccess: () => {
      toast({ title: `${config.label} deleted` });
      queryClient.invalidateQueries({ queryKey: [config.key, "list"] });
      router.push(`/${config.key}`);
    },
    onError: () => {
      toast({ title: "Delete failed", variant: "destructive" });
    }
  });
  const query = useQuery({
    queryKey: [config.key, id],
    queryFn: () => getResource<any>(config, id)
  });

  return (
    <div className="space-y-6">
      <Card>
        <CardHeader className="flex flex-row items-center justify-between">
          <CardTitle>{config.label} Details</CardTitle>
          <div className="flex gap-2">
            <RequirePermission permission={config.permissions?.edit ?? ""}>
              <Button variant="outline" onClick={() => router.push(`/${config.key}/${id}/edit`)}>
                Edit
              </Button>
            </RequirePermission>
            <RequirePermission permission={config.permissions?.delete ?? ""}>
              <Button variant="destructive" onClick={() => setOpen(true)}>
                Delete
              </Button>
            </RequirePermission>
          </div>
        </CardHeader>
        <CardContent>
          {query.isLoading ? (
            <p className="text-sm text-muted-foreground">Loading...</p>
          ) : query.isError ? (
            <p className="text-sm text-destructive">Failed to load record.</p>
          ) : (
            <pre className="whitespace-pre-wrap rounded-md bg-muted p-4 text-sm">
              {JSON.stringify(query.data, null, 2)}
            </pre>
          )}
        </CardContent>
      </Card>

      <Dialog open={open} onOpenChange={setOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Delete {config.label}</DialogTitle>
            <DialogDescription>Are you sure you want to delete this record?</DialogDescription>
          </DialogHeader>
          <DialogFooter>
            <Button variant="outline" onClick={() => setOpen(false)}>
              Cancel
            </Button>
            <Button
              variant="destructive"
              onClick={() => deleteMutation.mutate()}
              disabled={deleteMutation.isPending}
            >
              Confirm
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}

export function ResourceFormPage({
  config,
  id
}: {
  config: ResourceConfig;
  id?: string;
}) {
  const router = useRouter();
  const queryClient = useQueryClient();
  const query = useQuery({
    queryKey: [config.key, id],
    queryFn: () => (id ? getResource<any>(config, id) : Promise.resolve(null)),
    enabled: Boolean(id)
  });

  const mutation = useMutation({
    mutationFn: (values: Record<string, unknown>) =>
      id ? updateResource(config, values) : createResource(config, values),
    onSuccess: () => {
      toast({ title: `${config.label} saved` });
      queryClient.invalidateQueries({ queryKey: [config.key, "list"] });
      router.push(`/${config.key}`);
    },
    onError: () => {
      toast({ title: "Save failed", variant: "destructive" });
    }
  });

  return (
    <Card>
      <CardHeader>
        <CardTitle>{id ? `Edit ${config.label}` : `Create ${config.label}`}</CardTitle>
      </CardHeader>
      <CardContent>
        {id && query.isLoading ? (
          <p className="text-sm text-muted-foreground">Loading...</p>
        ) : (
          <AutoForm
            schemaName={(id ? config.updateSchema : config.createSchema) ?? ""}
            defaultValues={query.data ?? undefined}
            onSubmit={(values) => mutation.mutate(values)}
            submitLabel={id ? "Update" : "Create"}
          />
        )}
      </CardContent>
    </Card>
  );
}
