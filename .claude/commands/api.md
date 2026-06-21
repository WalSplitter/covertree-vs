# Design and Implement an API Endpoint

Design and implement the following API endpoint: $ARGUMENTS

## Before starting

- Check existing endpoints in `apps/web-backend/src/routes/`
- Review shared types in `shared/types/src/index.ts` (`IApiResponse`, `IPaginatedResponse`)
- Confirm auth requirements (JWT via `apps/web-backend/src/middleware/auth.ts` or public)

## Endpoint specification template

```
Method + Path:  POST /api/v1/<resource>/<action>
Purpose:        [what it does]
Auth:           [JWT required | public | admin only]
Rate limiting:  [e.g. 5 req/10min per IP]

Request body:
  field: type (constraints)

Success response (200/201):
  { success: true, data: { ... } }   ← IApiResponse<T> wrapper

Error cases:
  400 → Validation failed (Joi)
  401 → Missing/invalid token
  403 → Insufficient permissions
  404 → Resource not found
  409 → Conflict (duplicate, etc.)
  429 → Rate limited
  500 → Server error (logged by Pino)
```

## Implementation requirements

- **Validation**: Joi schema in middleware or route handler
- **Security**: Helmet headers already configured globally
- **Logging**: Pino logger for all operations
- **DB queries**: Parameterized only – never string concatenation
- **Caching**: Redis for frequently-read data (optional, configured in docker-compose)

## REST conventions

```
GET    /api/v1/users          # List
POST   /api/v1/users          # Create
GET    /api/v1/users/:id      # Single
PUT    /api/v1/users/:id      # Replace
PATCH  /api/v1/users/:id      # Partial update
DELETE /api/v1/users/:id      # Delete
```

## Test with

```bash
npm run dev -w @base-template/web-backend
curl -X POST http://localhost:3000/api/v1/<endpoint> \
  -H "Content-Type: application/json" \
  -d '{ ... }'
```

## Deliverables

1. Express route handler in `apps/web-backend/src/routes/`
2. Request/response types added to `shared/types/src/index.ts`
3. Unit tests covering success + all error cases (Vitest)
4. Conventional commit: `feat(api): <description>`
