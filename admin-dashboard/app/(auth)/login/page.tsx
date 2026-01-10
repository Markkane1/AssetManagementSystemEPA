"use client";

import { useRouter } from "next/navigation";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";

import { useAuthStore } from "@/src/auth/store";
import { apiFetch } from "@/src/api/client";
import type { components } from "@/src/api/types";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/src/components/ui/card";
import { Button } from "@/src/components/ui/button";
import { Input } from "@/src/components/ui/input";
import { Label } from "@/src/components/ui/label";
import { toast } from "@/src/components/ui/use-toast";

const schema = z.object({
  username: z.string().min(1, "Username is required"),
  password: z.string().min(1, "Password is required")
});

type LoginFormValues = z.infer<typeof schema>;

export default function LoginPage() {
  const router = useRouter();
  const setSession = useAuthStore((state) => state.setSession);
  const form = useForm<LoginFormValues>({
    resolver: zodResolver(schema)
  });

  const onSubmit = async (values: LoginFormValues) => {
    try {
      const response = await apiFetch<components["schemas"]["AuthResponseDto"]>(
        "/api/Auth/login",
        {
          method: "POST",
          body: JSON.stringify(values)
        },
        { retryOnUnauthorized: false }
      );

      setSession({
        token: response.token ?? null,
        refreshToken: response.refreshToken ?? null,
        expiresAt: response.expiresAt ?? null,
        user: response.user ?? null
      });

      const me = await apiFetch<components["schemas"]["UserInfoDto"]>("/api/Auth/me", {
        method: "GET"
      });

      setSession({ user: me });
      router.push("/dashboard");
    } catch (error: any) {
      toast({
        title: "Login failed",
        description: error?.message ?? "Unable to sign in.",
        variant: "destructive"
      });
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-muted/40 p-6">
      <Card className="w-full max-w-md">
        <CardHeader>
          <CardTitle>Asset Management Admin</CardTitle>
          <CardDescription>Sign in to manage assets and operations.</CardDescription>
        </CardHeader>
        <CardContent>
          <form className="space-y-4" onSubmit={form.handleSubmit(onSubmit)}>
            <div className="space-y-2">
              <Label htmlFor="username">Username</Label>
              <Input id="username" {...form.register("username")} />
              {form.formState.errors.username && (
                <p className="text-xs text-destructive">{form.formState.errors.username.message}</p>
              )}
            </div>
            <div className="space-y-2">
              <Label htmlFor="password">Password</Label>
              <Input id="password" type="password" {...form.register("password")} />
              {form.formState.errors.password && (
                <p className="text-xs text-destructive">{form.formState.errors.password.message}</p>
              )}
            </div>
            <Button type="submit" className="w-full">
              Sign in
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
