import { AppShell } from "@/src/components/layout/app-shell";
import { AuthGuard } from "@/src/auth/auth-guard";

export default function DashboardLayout({ children }: { children: React.ReactNode }) {
  return (
    <AuthGuard>
      <AppShell>{children}</AppShell>
    </AuthGuard>
  );
}
