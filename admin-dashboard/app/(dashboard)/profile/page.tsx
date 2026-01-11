"use client";

import { useAuthStore } from "@/src/auth/store";
import { Card, CardContent, CardHeader, CardTitle } from "@/src/components/ui/card";

export default function ProfilePage() {
  const user = useAuthStore((state) => state.user);

  return (
    <Card>
      <CardHeader>
        <CardTitle>My Profile</CardTitle>
      </CardHeader>
      <CardContent>
        <pre className="whitespace-pre-wrap rounded-md bg-muted p-4 text-sm">
          {JSON.stringify(user, null, 2)}
        </pre>
      </CardContent>
    </Card>
  );
}
