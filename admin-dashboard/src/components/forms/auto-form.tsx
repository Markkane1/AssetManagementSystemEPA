"use client";

import { useMemo } from "react";
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";

import { getSchema, type OpenApiSchema } from "@/src/api/openapi";
import { zodFromSchema } from "@/src/lib/schema";
import { Button } from "@/src/components/ui/button";
import { Input } from "@/src/components/ui/input";
import { Label } from "@/src/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/src/components/ui/select";
import { Switch } from "@/src/components/ui/switch";
import { Textarea } from "@/src/components/ui/textarea";

export type AutoFormProps = {
  schemaName: string;
  defaultValues?: Record<string, unknown>;
  onSubmit: (values: Record<string, unknown>) => void | Promise<void>;
  submitLabel?: string;
};

function resolveSchema(schema: OpenApiSchema): OpenApiSchema {
  if (schema.$ref) {
    const refName = schema.$ref.split("/").pop();
    if (refName) {
      return getSchema(refName) ?? schema;
    }
  }
  return schema;
}

function FieldInput({
  name,
  schema
}: {
  name: string;
  schema: OpenApiSchema;
}) {
  const resolved = resolveSchema(schema);
  if (resolved.enum) {
    return (
      <Controller
        name={name}
        render={({ field }) => (
          <Select onValueChange={field.onChange} defaultValue={field.value as string}>
            <SelectTrigger>
              <SelectValue placeholder={`Select ${name}`} />
            </SelectTrigger>
            <SelectContent>
              {resolved.enum?.map((value) => (
                <SelectItem key={String(value)} value={String(value)}>
                  {String(value)}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        )}
      />
    );
  }

  if (resolved.type === "boolean") {
    return (
      <Controller
        name={name}
        render={({ field }) => (
          <Switch checked={Boolean(field.value)} onCheckedChange={field.onChange} />
        )}
      />
    );
  }

  if (resolved.type === "integer" || resolved.type === "number") {
    return (
      <Controller
        name={name}
        render={({ field }) => (
          <Input
            type="number"
            value={field.value as number | undefined}
            onChange={(event) => field.onChange(event.target.valueAsNumber)}
          />
        )}
      />
    );
  }

  if (resolved.type === "array" || resolved.type === "object") {
    return (
      <Controller
        name={name}
        render={({ field }) => (
          <Textarea
            value={field.value ? JSON.stringify(field.value, null, 2) : ""}
            onChange={(event) => {
              try {
                const parsed = JSON.parse(event.target.value);
                field.onChange(parsed);
              } catch {
                field.onChange(event.target.value);
              }
            }}
            placeholder="Enter JSON"
          />
        )}
      />
    );
  }

  const inputType = resolved.format === "date-time" ? "datetime-local" : "text";
  return (
    <Controller
      name={name}
      render={({ field }) => (
        <Input
          type={inputType}
          value={(field.value as string | undefined) ?? ""}
          onChange={field.onChange}
        />
      )}
    />
  );
}

export function AutoForm({ schemaName, defaultValues, onSubmit, submitLabel = "Save" }: AutoFormProps) {
  const schema = getSchema(schemaName);
  const zodSchema = useMemo(() => (schema ? zodFromSchema(schema) : z.object({})), [schema]);
  const form = useForm<Record<string, unknown>>({
    resolver: zodResolver(zodSchema),
    defaultValues
  });

  if (!schema || !schema.properties) {
    return <p className="text-sm text-muted-foreground">No schema available.</p>;
  }

  return (
    <form
      className="space-y-6"
      onSubmit={form.handleSubmit(async (values) => {
        await onSubmit(values);
      })}
    >
      <div className="grid gap-6 md:grid-cols-2">
        {Object.entries(schema.properties).map(([name, fieldSchema]) => (
          <div key={name} className="space-y-2">
            <Label htmlFor={name}>{name}</Label>
            <FieldInput name={name} schema={fieldSchema} />
            {form.formState.errors[name] && (
              <p className="text-xs text-destructive">
                {String(form.formState.errors[name]?.message ?? "Invalid value")}
              </p>
            )}
          </div>
        ))}
      </div>
      <Button type="submit">{submitLabel}</Button>
    </form>
  );
}
