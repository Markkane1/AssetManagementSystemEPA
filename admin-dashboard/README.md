# Asset Management Admin Dashboard

A modern admin dashboard built with Next.js App Router, TailwindCSS, shadcn/ui, TanStack Query/Table, React Hook Form + Zod, Zustand, and Recharts.

## Setup

```bash
cd admin-dashboard
npm install
```

## Environment

Create `.env.local` from `.env.example`:

```bash
cp .env.example .env.local
```

Set the API base URL to your backend:

```
NEXT_PUBLIC_API_BASE_URL=http://localhost:5000
```

## Development

```bash
npm run dev
```

## Generate Types (Optional)

If you have access to the npm registry, you can regenerate the OpenAPI types:

```bash
npm run generate:types
```

The OpenAPI spec is bundled as `openapi.json` at the dashboard root.
