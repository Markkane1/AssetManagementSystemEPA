import spec from "../../openapi.json";

export type OpenApiSchema = {
  type?: string;
  format?: string;
  enum?: Array<string | number>;
  properties?: Record<string, OpenApiSchema>;
  required?: string[];
  items?: OpenApiSchema;
  nullable?: boolean;
  $ref?: string;
};

export type OpenApiSpec = typeof spec;

export function getSchemaByRef(ref: string): OpenApiSchema | null {
  const path = ref.replace("#/", "").split("/");
  let current: any = spec;
  for (const part of path) {
    current = current?.[part];
    if (!current) return null;
  }
  return current as OpenApiSchema;
}

export function getSchema(name: string): OpenApiSchema | null {
  return (spec as any).components?.schemas?.[name] ?? null;
}
