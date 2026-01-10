import { z } from "zod";

import { getSchemaByRef, type OpenApiSchema } from "@/src/api/openapi";

function resolveSchema(schema: OpenApiSchema): OpenApiSchema {
  if (schema.$ref) {
    return getSchemaByRef(schema.$ref) ?? schema;
  }
  return schema;
}

export function zodFromSchema(schema: OpenApiSchema): z.ZodTypeAny {
  const resolved = resolveSchema(schema);
  if (resolved.enum) {
    return z.union(resolved.enum.map((value) => z.literal(value)) as [z.ZodTypeAny, ...z.ZodTypeAny[]]);
  }
  switch (resolved.type) {
    case "string":
      return z.string();
    case "integer":
    case "number":
      return z.number();
    case "boolean":
      return z.boolean();
    case "array":
      return z.array(resolved.items ? zodFromSchema(resolved.items) : z.any());
    case "object":
    default: {
      if (resolved.properties) {
        const shape: Record<string, z.ZodTypeAny> = {};
        const required = new Set(resolved.required ?? []);
        Object.entries(resolved.properties).forEach(([key, prop]) => {
          const propSchema = zodFromSchema(prop);
          shape[key] = required.has(key) ? propSchema : propSchema.optional();
        });
        return z.object(shape);
      }
      return z.record(z.any());
    }
  }
}
