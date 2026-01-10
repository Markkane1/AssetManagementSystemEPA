import { notFound } from "next/navigation";

import { getResourceConfig } from "@/src/features/resources/resource-config";
import { ResourceDetailPage } from "@/src/features/resources/resource-pages";

export default function ResourceDetail({ params }: { params: { resource: string; id: string } }) {
  const config = getResourceConfig(params.resource);
  if (!config) return notFound();
  return <ResourceDetailPage config={config} id={params.id} />;
}
