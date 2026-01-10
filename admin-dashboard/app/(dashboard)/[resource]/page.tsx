import { notFound } from "next/navigation";

import { getResourceConfig } from "@/src/features/resources/resource-config";
import { ResourceListPage } from "@/src/features/resources/resource-pages";

export default function ResourceList({ params }: { params: { resource: string } }) {
  const config = getResourceConfig(params.resource);
  if (!config) return notFound();
  return <ResourceListPage config={config} />;
}
