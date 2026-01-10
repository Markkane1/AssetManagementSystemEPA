import { notFound } from "next/navigation";

import { getResourceConfig } from "@/src/features/resources/resource-config";
import { ResourceFormPage } from "@/src/features/resources/resource-pages";

export default function ResourceCreate({ params }: { params: { resource: string } }) {
  const config = getResourceConfig(params.resource);
  if (!config) return notFound();
  return <ResourceFormPage config={config} />;
}
