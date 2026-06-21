# Programming Skills Guide

Comprehensive reference for implementing common programming patterns and techniques used in this project.

## Table of Contents

1. [Type Safety & TypeScript](#type-safety--typescript)
2. [Async Programming](#async-programming)
3. [Error Handling](#error-handling)
4. [API Integration](#api-integration)
5. [Data Validation](#data-validation)
6. [Caching Strategies](#caching-strategies)
7. [Testing Patterns](#testing-patterns)
8. [Performance Optimization](#performance-optimization)
9. [Security Implementation](#security-implementation)
10. [Design Patterns](#design-patterns)

## Type Safety & TypeScript

### Strict Type Definitions

```typescript
// Define precise types, avoid 'any'
interface IUserProfile {
  id: string;
  email: string;
  name: string;
  createdAt: Date;
  metadata?: Record<string, unknown>;
}

// Use discriminated unions for type safety
type ApiResponse<T> = { success: true; data: T } | { success: false; error: string; code: number };

// Use const assertions for type narrowing
const ROLES = {
  ADMIN: 'admin',
  USER: 'user',
  GUEST: 'guest',
} as const;

type UserRole = (typeof ROLES)[keyof typeof ROLES];
```

### Generics & Reusability

```typescript
// Generic repository pattern
interface IRepository<T> {
  findById(id: string): Promise<T | null>;
  create(data: Partial<T>): Promise<T>;
  update(id: string, data: Partial<T>): Promise<T>;
  delete(id: string): Promise<void>;
}

class BaseRepository<T extends { id: string }> implements IRepository<T> {
  constructor(private readonly collection: T[]) {}

  async findById(id: string): Promise<T | null> {
    return this.collection.find((item) => item.id === id) ?? null;
  }

  async create(data: Partial<T>): Promise<T> {
    const item = { ...data, id: generateId() } as T;
    this.collection.push(item);
    return item;
  }

  async update(id: string, data: Partial<T>): Promise<T> {
    const index = this.collection.findIndex((item) => item.id === id);
    if (index === -1) throw new NotFoundError();
    this.collection[index] = { ...this.collection[index], ...data };
    return this.collection[index];
  }

  async delete(id: string): Promise<void> {
    const index = this.collection.findIndex((item) => item.id === id);
    if (index === -1) throw new NotFoundError();
    this.collection.splice(index, 1);
  }
}
```

## Async Programming

### Promise Patterns

```typescript
// 1. Sequential execution
async function processSequential(items: string[]): Promise<void> {
  for (const item of items) {
    await processItem(item);
  }
}

// 2. Parallel execution (when order doesn't matter)
async function processParallel(items: string[]): Promise<void> {
  await Promise.all(items.map((item) => processItem(item)));
}

// 3. Concurrent with limit
async function processConcurrent(items: string[], concurrency: number): Promise<void> {
  for (let i = 0; i < items.length; i += concurrency) {
    const batch = items.slice(i, i + concurrency);
    await Promise.all(batch.map((item) => processItem(item)));
  }
}

// 4. Race condition handling
async function fetchWithTimeout(url: string, timeout: number): Promise<Response> {
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), timeout);

  try {
    return await fetch(url, { signal: controller.signal });
  } finally {
    clearTimeout(timeoutId);
  }
}
```

### Retry Logic

```typescript
async function withRetry<T>(
  fn: () => Promise<T>,
  maxAttempts: number = 3,
  baseDelay: number = 1000
): Promise<T> {
  let lastError: Error | null = null;

  for (let attempt = 0; attempt < maxAttempts; attempt++) {
    try {
      return await fn();
    } catch (error) {
      lastError = error as Error;
      if (attempt < maxAttempts - 1) {
        // Exponential backoff
        const delay = baseDelay * Math.pow(2, attempt);
        await new Promise((resolve) => setTimeout(resolve, delay));
      }
    }
  }

  throw lastError || new Error('Max retries exceeded');
}

// Usage
const data = await withRetry(() => fetchUserData(userId), 3);
```

## Error Handling

### Custom Error Classes

```typescript
// Base error class
abstract class AppError extends Error {
  abstract readonly statusCode: number;
  abstract readonly isOperational: boolean;

  constructor(message: string) {
    super(message);
    Object.setPrototypeOf(this, new.target.prototype);
    Error.captureStackTrace(this, this.constructor);
  }
}

// Specific error types
class ValidationError extends AppError {
  readonly statusCode = 400;
  readonly isOperational = true;

  constructor(
    public readonly field: string,
    message: string
  ) {
    super(message);
  }
}

class NotFoundError extends AppError {
  readonly statusCode = 404;
  readonly isOperational = true;

  constructor(resource: string, identifier: string) {
    super(`${resource} with identifier "${identifier}" not found`);
  }
}

class AuthenticationError extends AppError {
  readonly statusCode = 401;
  readonly isOperational = true;

  constructor(message: string = 'Authentication required') {
    super(message);
  }
}

class InternalServerError extends AppError {
  readonly statusCode = 500;
  readonly isOperational = false;

  constructor(message: string = 'Internal server error') {
    super(message);
  }
}
```

### Error Handling Middleware

```typescript
// Express error handler
app.use((error: Error, req: Request, res: Response, next: NextFunction) => {
  if (error instanceof AppError) {
    return res.status(error.statusCode).json({
      error: {
        message: error.message,
        type: error.constructor.name,
      },
    });
  }

  // Log unexpected errors
  logger.error('Unexpected error:', error);

  res.status(500).json({
    error: {
      message: 'Internal server error',
      type: 'InternalServerError',
    },
  });
});
```

## API Integration

### HTTP Client Wrapper

```typescript
interface IHttpClient {
  get<T>(url: string, config?: RequestConfig): Promise<T>;
  post<T>(url: string, data: unknown, config?: RequestConfig): Promise<T>;
  put<T>(url: string, data: unknown, config?: RequestConfig): Promise<T>;
  delete<T>(url: string, config?: RequestConfig): Promise<T>;
}

class HttpClient implements IHttpClient {
  constructor(
    private readonly baseUrl: string,
    private readonly timeout: number = 5000
  ) {}

  async get<T>(url: string, config?: RequestConfig): Promise<T> {
    const response = await fetch(this.buildUrl(url), {
      method: 'GET',
      headers: this.buildHeaders(config?.headers),
      signal: AbortSignal.timeout(this.timeout),
    });
    return this.handleResponse<T>(response);
  }

  async post<T>(url: string, data: unknown, config?: RequestConfig): Promise<T> {
    const response = await fetch(this.buildUrl(url), {
      method: 'POST',
      headers: this.buildHeaders(config?.headers),
      body: JSON.stringify(data),
      signal: AbortSignal.timeout(this.timeout),
    });
    return this.handleResponse<T>(response);
  }

  private buildUrl(path: string): string {
    return `${this.baseUrl}${path}`;
  }

  private buildHeaders(custom?: Record<string, string>): HeadersInit {
    return {
      'Content-Type': 'application/json',
      ...custom,
    };
  }

  private async handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
      const error = await response.json();
      throw new HttpError(response.status, error.message);
    }
    return response.json() as Promise<T>;
  }
}
```

## Data Validation

### Schema Validation

```typescript
// Using zod for runtime validation
import { z } from 'zod';

const UserSchema = z.object({
  id: z.string().uuid(),
  email: z.string().email(),
  name: z.string().min(1).max(100),
  age: z.number().int().min(0).max(150).optional(),
  roles: z.array(z.enum(['admin', 'user', 'guest'])).default(['user']),
});

type User = z.infer<typeof UserSchema>;

function validateUser(data: unknown): User {
  return UserSchema.parse(data);
}

// Validate and provide detailed errors
try {
  const user = validateUser(jsonData);
} catch (error) {
  if (error instanceof z.ZodError) {
    const errorMessage = error.errors
      .map((err) => `${err.path.join('.')}: ${err.message}`)
      .join(', ');
    throw new ValidationError('userData', errorMessage);
  }
}
```

## Caching Strategies

### Memory Cache with TTL

```typescript
interface ICacheEntry<T> {
  value: T;
  expiresAt: number;
}

class MemoryCache<T> {
  private cache = new Map<string, ICacheEntry<T>>();

  set(key: string, value: T, ttlMs: number = 5 * 60 * 1000): void {
    this.cache.set(key, {
      value,
      expiresAt: Date.now() + ttlMs,
    });
  }

  get(key: string): T | null {
    const entry = this.cache.get(key);
    if (!entry) return null;

    if (Date.now() > entry.expiresAt) {
      this.cache.delete(key);
      return null;
    }

    return entry.value;
  }

  clear(): void {
    this.cache.clear();
  }
}

// Usage with decorator
function Cacheable(ttlMs: number = 5 * 60 * 1000) {
  const cache = new MemoryCache<unknown>();

  return function (
    target: unknown,
    propertyKey: string,
    descriptor: PropertyDescriptor
  ): PropertyDescriptor {
    const originalMethod = descriptor.value;

    descriptor.value = async function (...args: unknown[]) {
      const cacheKey = `${propertyKey}:${JSON.stringify(args)}`;
      const cached = cache.get(cacheKey);

      if (cached !== null) {
        return cached;
      }

      const result = await originalMethod.apply(this, args);
      cache.set(cacheKey, result, ttlMs);
      return result;
    };

    return descriptor;
  };
}
```

## Testing Patterns

### Unit Test Structure

```typescript
describe('UserService', () => {
  let userService: UserService;
  let mockRepository: Mock<IUserRepository>;

  beforeEach(() => {
    mockRepository = createMock<IUserRepository>();
    userService = new UserService(mockRepository);
  });

  describe('getUserProfile', () => {
    it('should return user profile when user exists', async () => {
      // Arrange
      const userId = 'user-123';
      const expectedUser = { id: userId, name: 'John Doe', email: 'john@example.com' };
      mockRepository.findById.mockResolvedValue(expectedUser);

      // Act
      const result = await userService.getUserProfile(userId);

      // Assert
      expect(result).toEqual(expectedUser);
      expect(mockRepository.findById).toHaveBeenCalledWith(userId);
    });

    it('should throw NotFoundError when user does not exist', async () => {
      // Arrange
      mockRepository.findById.mockResolvedValue(null);

      // Act & Assert
      await expect(userService.getUserProfile('invalid-id')).rejects.toThrow(NotFoundError);
    });
  });
});
```

## Performance Optimization

### Debouncing & Throttling

```typescript
// Debounce: Execute after delay, cancel if called again
function debounce<T extends (...args: unknown[]) => unknown>(
  fn: T,
  delayMs: number
): (...args: Parameters<T>) => void {
  let timeoutId: ReturnType<typeof setTimeout> | null = null;

  return (...args: Parameters<T>) => {
    if (timeoutId) clearTimeout(timeoutId);
    timeoutId = setTimeout(() => fn(...args), delayMs);
  };
}

// Throttle: Execute at most once per interval
function throttle<T extends (...args: unknown[]) => unknown>(
  fn: T,
  intervalMs: number
): (...args: Parameters<T>) => void {
  let lastCallTime = 0;

  return (...args: Parameters<T>) => {
    const now = Date.now();
    if (now - lastCallTime >= intervalMs) {
      lastCallTime = now;
      fn(...args);
    }
  };
}
```

## Security Implementation

### Password Hashing

```typescript
import bcrypt from 'bcrypt';

const SALT_ROUNDS = 12;

async function hashPassword(password: string): Promise<string> {
  return bcrypt.hash(password, SALT_ROUNDS);
}

async function verifyPassword(password: string, hash: string): Promise<boolean> {
  return bcrypt.compare(password, hash);
}
```

### JWT Token Management

```typescript
import jwt from 'jsonwebtoken';

interface ITokenPayload {
  userId: string;
  role: string;
  iat?: number;
  exp?: number;
}

class TokenService {
  constructor(
    private readonly jwtSecret: string,
    private readonly tokenExpiryMs: number = 24 * 60 * 60 * 1000
  ) {}

  generateToken(payload: Omit<ITokenPayload, 'iat' | 'exp'>): string {
    return jwt.sign(payload, this.jwtSecret, {
      expiresIn: this.tokenExpiryMs / 1000,
    });
  }

  verifyToken(token: string): ITokenPayload {
    try {
      return jwt.verify(token, this.jwtSecret) as ITokenPayload;
    } catch (error) {
      throw new AuthenticationError('Invalid or expired token');
    }
  }
}
```

## Design Patterns

### Dependency Injection

```typescript
// Service container
class Container {
  private services = new Map<string, unknown>();

  register<T>(key: string, factory: () => T): void {
    this.services.set(key, factory());
  }

  get<T>(key: string): T {
    const service = this.services.get(key);
    if (!service) throw new Error(`Service "${key}" not found`);
    return service as T;
  }
}

// Usage
const container = new Container();
container.register('userRepository', () => new UserRepository());
container.register('userService', () => new UserService(container.get('userRepository')));
```

### Observer Pattern

```typescript
interface IObserver<T> {
  update(data: T): void;
}

class Observable<T> {
  private observers: Set<IObserver<T>> = new Set();

  subscribe(observer: IObserver<T>): () => void {
    this.observers.add(observer);
    // Return unsubscribe function
    return () => this.observers.delete(observer);
  }

  notify(data: T): void {
    this.observers.forEach((observer) => observer.update(data));
  }
}
```

### Factory Pattern

```typescript
interface ILogger {
  info(message: string): void;
  error(message: string): void;
}

class LoggerFactory {
  static create(type: 'console' | 'file' | 'database'): ILogger {
    switch (type) {
      case 'console':
        return new ConsoleLogger();
      case 'file':
        return new FileLogger();
      case 'database':
        return new DatabaseLogger();
      default:
        throw new Error(`Unknown logger type: ${type}`);
    }
  }
}
```

---

**Last Updated**: 2026
**Version**: 1.0
