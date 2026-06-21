# Shared Libraries & Package Patterns

Domain-specific guidance for the `/shared/*` workspace.

## Principle: Define once, use everywhere

All cross-workspace types, utilities, and constants live in `shared/`. Never duplicate type definitions across workspaces.

## shared/types structure

```
shared/types/src/
├── index.ts        ← All public exports
├── api.ts          ← IApiResponse, IPaginatedResponse, IErrorResponse
├── user.ts         ← IUser, IUserProfile, IAuthPayload
├── common.ts       ← Utility types (Maybe<T>, Result<T, E>, etc.)
└── constants.ts    ← Shared enums and constants
```

**Key types already available:**

```typescript
IApiResponse<T>; // { success: true, data: T } | { success: false, error, code }
IPaginatedResponse<T>; // { items: T[], total, hasMore, pageInfo }
IUser; // Core user shape
IAuthPayload; // JWT payload
```

## Adding new shared types

1. Define in the appropriate file under `shared/types/src/`
2. Export from `shared/types/src/index.ts`
3. Run `npm run type-check --workspaces` to verify no breakage
4. No business logic in shared types – interfaces and types only

## Shared utilities

```
shared/utils/src/
├── retry.ts        ← withRetry(fn, maxAttempts, baseDelay)
├── validation.ts   ← Common Zod schemas
├── formatting.ts   ← Date, currency, string formatters
└── errors.ts       ← Base AppError class hierarchy
```

## Package versioning in monorepo

- All workspaces reference shared packages via workspace protocol: `"@shared/types": "*"`
- After adding exports, run `npm install` from root to update symlinks
- Breaking changes to shared types require updating all consuming workspaces

## Testing shared code

```bash
npm run test -w shared/types
npm run test:coverage -w shared/types
# Shared code should have highest coverage (85%+) – it's used everywhere
```
