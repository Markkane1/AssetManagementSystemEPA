import type { Metadata } from "next";

import "@/src/styles/globals.css";
import { Providers } from "@/src/components/providers";

export const metadata: Metadata = {
  title: "Asset Management Admin",
  description: "Admin dashboard for asset management"
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" suppressHydrationWarning>
      <body>
        <Providers>{children}</Providers>
      </body>
    </html>
  );
}
