# Prompt: API & Integration Design - Base Project Template

Use this prompt template when designing APIs and integrations with GitHub Copilot.

## 📋 Before Designing API

Review:

- Existing endpoints in apps/web-backend/src/routes/
- Shared types in shared/types/src/index.ts (IApiResponse, IPaginatedResponse)
- [Code Standards](./.github/copilot-instructions.md) - Naming conventions
- Security: Helmet.js headers, CORS, rate limiting, input validation (Joi)

## Template

```
API Specification:
Endpoint: [METHOD] /api/v1/[RESOURCE]/[ACTION]
Workspace: apps/web-backend (or other service)
Purpose: [WHAT_DOES_IT_DO]
Use case: [WHEN_IS_IT_USED]

Request:
- Method: [GET/POST/PUT/DELETE/PATCH]
- Path: [/api/v1/path/with/:params]
- Query params: [OPTIONAL_FILTERS]
- Headers: Standard + Authorization (Bearer token if needed)
- Body schema: Use Joi validation, reference shared/types/
- Authentication: JWT (apps/web-backend/src/middleware/auth.ts) or none for public
- Rate limiting: Apply rate-limit middleware if needed
- Content-Type: application/json

Response:
- Success (200/201): Use IApiResponse<T> wrapper from shared/types/
```

{ success: true, data: {...}, message?: "..." }

```
- Error cases (structured error responses from error-handler.ts):
- 400: Validation error (Joi validation failed)
- 401: Unauthorized (missing/invalid token)
- 403: Forbidden (insufficient permissions)
- 404: Not found resource
- 409: Conflict (unique constraint, duplicate)
- 500: Server error (logged by Pino)

Implementation requirements:
- Input validation: Use Joi schemas, validate in middleware or route handler
- Authorization: Check JWT payload, verify user permissions
- Database operations: Use parameterized queries (PostgreSQL), prevent SQL injection
- External integrations: Document API calls, add error handling, timeouts
- Caching: Use Redis for frequently accessed data (optional, enable in docker-compose)
- Logging: Use Pino logger for all operations (apps/web-backend/src/utils/logger.ts)

Deliverables:
1. Express route handler in apps/web-backend/src/routes/ with Helmet security
2. Request/response types in shared/types/src/index.ts with JSDoc
3. Unit tests (vitest) covering success and error cases
4. Integration tests with mocked external services
5. Commit: feat(api): description of new endpoint
6. Optional: Add OpenAPI/Swagger documentation

Test with:
\`\`\`bash
npm run dev -w @base-template/web-backend  # Start backend on :3000
curl http://localhost:3000/api/v1/endpoint
\`\`\`
```

## Example Usage

```
API Specification:
Endpoint: POST /api/users/register
Purpose: Create new user account with email and password
Use case: User self-registration flow

Request:
- Method: POST
- Path: /api/users/register
- Body schema:
  {
    email: string (email format, required)
    password: string (min 8 chars, required)
    name: string (1-100 chars, required)
  }
- Authentication: None (public endpoint)
- Rate limiting: 5 requests per 10 minutes per IP

Response:
- Success (201):
  {
    userId: string (UUID)
    email: string
    name: string
    createdAt: ISO8601 timestamp
  }
- Error cases:
  - 400: Invalid email format or password too weak
  - 409: Email already registered
  - 429: Rate limit exceeded
  - 500: Internal server error

Implementation requirements:
- Input validation: Email format, password strength, name length
- Authorization: None required
- Database operations: Check email uniqueness, hash password, insert user
- External integrations: Send verification email
- Caching: Cache email domain validation

Deliverables:
1. UserController.register() handler
2. IUserRegistrationRequest, IUserRegistrationResponse types
3. Unit tests for all validation scenarios
4. Integration test with email service mock
5. OpenAPI spec entry
```

## REST API Best Practices

### URL Design

```
✓ /api/v1/users                    # Resource collection
✓ /api/v1/users/:id               # Resource by ID
✓ /api/v1/users/:id/posts         # Related resources
✓ /api/v1/users?role=admin        # Query filters
✓ /api/v1/users?skip=10&take=20   # Pagination
✗ /api/getUsers                    # Avoid verb names
✗ /api/user-get-by-id              # Avoid method names
```

### HTTP Methods

```typescript
POST   /api/resource          // Create
GET    /api/resource          // List
GET    /api/resource/:id      // Retrieve single
PUT    /api/resource/:id      // Replace entire
PATCH  /api/resource/:id      // Partial update
DELETE /api/resource/:id      // Delete
```

### Status Codes

```
2xx Success
  200 OK              - GET, PUT, PATCH, DELETE successful
  201 Created         - POST successful, new resource created
  204 No Content      - Successful with no response body

4xx Client Error
  400 Bad Request     - Invalid input
  401 Unauthorized    - Authentication required
  403 Forbidden       - Authenticated but not authorized
  404 Not Found       - Resource doesn't exist
  409 Conflict        - Resource conflict (duplicate email, etc.)
  429 Too Many Requests - Rate limited

5xx Server Error
  500 Internal Server Error
  503 Service Unavailable
```

### Error Response Format

```typescript
interface IErrorResponse {
  error: {
    code: string;           // Machine-readable error code
    message: string;        // Human-readable message
    details?: {            // Optional detailed information
      [key: string]: unknown;
    };
    timestamp: string;      // ISO8601 timestamp
  };
}

// Example
{
  error: {
    code: 'VALIDATION_ERROR',
    message: 'Validation failed',
    details: {
      field: 'email',
      reason: 'Invalid email format'
    },
    timestamp: '2026-05-31T12:00:00Z'
  }
}
```

## GraphQL Integration

```typescript
// Type definition
type User {
  id: ID!
  email: String!
  name: String!
  createdAt: DateTime!
  posts: [Post!]!
}

// Query
query GetUser($id: ID!) {
  user(id: $id) {
    id
    email
    name
    posts {
      id
      title
    }
  }
}

// Resolver with proper error handling
const userResolver = {
  async user(parent: unknown, args: { id: string }) {
    try {
      const user = await userService.getUserById(args.id);
      if (!user) throw new NotFoundError('User', args.id);
      return user;
    } catch (error) {
      logger.error('GraphQL user query error:', error);
      throw new ApolloError('Failed to fetch user');
    }
  }
};
```
