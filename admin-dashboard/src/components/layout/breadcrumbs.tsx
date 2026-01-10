"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

export function Breadcrumbs() {
  const pathname = usePathname();
  const segments = pathname.split("/").filter(Boolean);

  return (
    <nav className="text-sm text-muted-foreground">
      <ol className="flex flex-wrap items-center gap-2">
        <li>
          <Link href="/dashboard" className="hover:text-foreground">
            Home
          </Link>
        </li>
        {segments.map((segment, idx) => {
          const href = "/" + segments.slice(0, idx + 1).join("/");
          return (
            <li key={href} className="flex items-center gap-2">
              <span>/</span>
              <Link href={href} className="hover:text-foreground capitalize">
                {segment.replace(/-/g, " ")}
              </Link>
            </li>
          );
        })}
      </ol>
    </nav>
  );
}
