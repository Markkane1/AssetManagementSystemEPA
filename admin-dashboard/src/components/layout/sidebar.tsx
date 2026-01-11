"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { LayoutDashboard, Package, Users, Settings, ClipboardList, FileBarChart2 } from "lucide-react";

import { cn } from "@/src/lib/utils";
import { useAuthStore } from "@/src/auth/store";
import { navItems } from "@/src/components/layout/navigation";

export function Sidebar({ collapsed }: { collapsed: boolean }) {
  const pathname = usePathname();
  const can = useAuthStore((state) => state.can);

  return (
    <aside
      className={cn(
        "flex h-full flex-col border-r bg-card transition-all",
        collapsed ? "w-16" : "w-64"
      )}
    >
      <div className="flex h-16 items-center justify-center border-b">
        <span className="text-lg font-semibold">{collapsed ? "AM" : "Asset Manager"}</span>
      </div>
      <nav className="flex-1 space-y-1 p-3">
        {navItems
          .filter((item) => !item.permission || can(item.permission))
          .map((item) => (
            <Link
              key={item.href}
              href={item.href}
              className={cn(
                "flex items-center gap-3 rounded-md px-3 py-2 text-sm font-medium hover:bg-muted",
                pathname === item.href && "bg-muted"
              )}
            >
              <item.icon className="h-4 w-4" />
              {!collapsed && <span>{item.label}</span>}
            </Link>
          ))}
      </nav>
    </aside>
  );
}

export const IconLibrary = {
  LayoutDashboard,
  Package,
  Users,
  Settings,
  ClipboardList,
  FileBarChart2
};
