# Backend & Microservices Patterns

Domain-specific guidance for the `/apps/*` workspace (standalone services).

## Service Architecture

```
apps/<service>/src/
├── domain/
│   ├── models/       ← Data models
│   └── services/     ← Business logic
├── infrastructure/
│   ├── repositories/ ← DB access (parameterized queries only)
│   ├── http-clients/ ← External API wrappers
│   └── config/       ← Environment config
├── api/
│   ├── routes/
│   ├── controllers/
│   └── middleware/
└── index.ts
```

## Repository Pattern

```typescript
// Always use repository pattern for data access
interface IUserRepository {
  findById(id: string): Promise<IUser | null>;
  create(data: Omit<IUser, 'id' | 'createdAt'>): Promise<IUser>;
  update(id: string, data: Partial<IUser>): Promise<IUser>;
  delete(id: string): Promise<void>;
}

// Never raw SQL in services – always through repository
class UserService {
  constructor(private readonly repo: IUserRepository) {}
}
```

## Database (PostgreSQL)

- Parameterized queries only (never string interpolation)
- Use transactions for multi-step operations
- Add indexes for all foreign keys and frequently filtered columns
- Use `SELECT FOR UPDATE` for race-condition-prone flows (e.g. unique checks)

```bash
make docker-up   # Start PostgreSQL on :5432, Redis on :6379
```

## Async patterns

```typescript
// Concurrent with limit (avoid overwhelming DB)
async function processBatch(items: string[], concurrency = 5) {
  for (let i = 0; i < items.length; i += concurrency) {
    await Promise.all(items.slice(i, i + concurrency).map(processItem));
  }
}

// Retry with exponential backoff (already in shared/utils)
const result = await withRetry(() => externalApiCall(), 3);
```

## Inter-service communication

- REST for synchronous calls – use `HttpClient` wrapper from shared utils
- Always set timeouts on external calls (`AbortSignal.timeout(5000)`)
- Log all outbound calls with Pino

## Testing services

```bash
npm run test -w apps/<service>
npm run test:coverage -w apps/<service>
```
