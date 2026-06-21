# Shared Code & Libraries

Dedicated directory for shared utilities, types, and libraries used across multiple projects.

## Project Structure

```
shared/
├── types/                     # Shared TypeScript types
│   ├── domain/               # Domain models
│   │   ├── user.ts
│   │   ├── product.ts
│   │   └── order.ts
│   ├── api/                  # API contracts
│   │   ├── requests.ts
│   │   ├── responses.ts
│   │   └── errors.ts
│   ├── index.ts              # Export all types
│   └── package.json
│
├── utils/                     # Utility functions
│   ├── string-utils.ts
│   ├── date-utils.ts
│   ├── validation.ts
│   ├── formatting.ts
│   ├── index.ts
│   └── package.json
│
├── constants/                 # Shared constants
│   ├── status-codes.ts
│   ├── error-messages.ts
│   ├── validation-rules.ts
│   ├── index.ts
│   └── package.json
│
├── logger/                    # Logging utilities
│   ├── logger.ts
│   ├── formatters.ts
│   ├── transports.ts
│   ├── index.ts
│   └── package.json
│
├── errors/                    # Custom error classes
│   ├── app-error.ts
│   ├── validation-error.ts
│   ├── not-found-error.ts
│   ├── index.ts
│   └── package.json
│
└── testing/                   # Testing utilities
    ├── mocks/
    ├── fixtures/
    ├── helpers.ts
    ├── index.ts
    └── package.json
```

## Shared Type System

### Domain Models

```typescript
// shared/types/domain/user.ts
export interface IUser {
  id: string;
  email: string;
  name: string;
  role: UserRole;
  status: UserStatus;
  createdAt: Date;
  updatedAt: Date;
}

export type UserRole = 'admin' | 'user' | 'guest';
export type UserStatus = 'active' | 'inactive' | 'suspended';

export interface IUserCreateInput {
  email: string;
  name: string;
  password: string;
}

export interface IUserUpdateInput {
  name?: string;
  email?: string;
  role?: UserRole;
}
```

### API Contracts

```typescript
// shared/types/api/requests.ts
export interface IListRequest {
  skip?: number;
  take?: number;
  sortBy?: string;
  sortOrder?: 'asc' | 'desc';
}

export interface IUserListRequest extends IListRequest {
  role?: string;
  status?: string;
}

// shared/types/api/responses.ts
export interface IPaginatedResponse<T> {
  items: T[];
  total: number;
  hasMore: boolean;
  page: number;
  pageSize: number;
}

export interface IErrorResponse {
  error: {
    code: string;
    message: string;
    details?: Record<string, unknown>;
  };
}
```

## Shared Utilities

### String Utilities

```typescript
// shared/utils/string-utils.ts
export class StringUtils {
  static pascalCase(str: string): string {
    return str
      .split(/[-_\s]/g)
      .map((word) => word.charAt(0).toUpperCase() + word.slice(1))
      .join('');
  }

  static camelCase(str: string): string {
    const pascal = this.pascalCase(str);
    return pascal.charAt(0).toLowerCase() + pascal.slice(1);
  }

  static kebabCase(str: string): string {
    return str
      .replace(/([a-z])([A-Z])/g, '$1-$2')
      .replace(/[\s_]+/g, '-')
      .toLowerCase();
  }

  static snakeCase(str: string): string {
    return str
      .replace(/([a-z])([A-Z])/g, '$1_$2')
      .replace(/[\s-]+/g, '_')
      .toLowerCase();
  }

  static truncate(str: string, length: number): string {
    return str.length > length ? `${str.substring(0, length)}...` : str;
  }
}
```

### Validation Utilities

```typescript
// shared/utils/validation.ts
import { z } from 'zod';

export class Validators {
  static readonly EMAIL_SCHEMA = z.string().email();
  static readonly PASSWORD_SCHEMA = z.string().min(8).max(128);
  static readonly UUID_SCHEMA = z.string().uuid();
  static readonly URL_SCHEMA = z.string().url();

  static validateEmail(email: string): boolean {
    return this.EMAIL_SCHEMA.safeParse(email).success;
  }

  static validatePassword(password: string): boolean {
    return this.PASSWORD_SCHEMA.safeParse(password).success;
  }

  static validateUUID(uuid: string): boolean {
    return this.UUID_SCHEMA.safeParse(uuid).success;
  }

  static getPasswordStrength(password: string): 'weak' | 'medium' | 'strong' {
    let strength = 0;
    if (password.length >= 8) strength++;
    if (/[a-z]/.test(password)) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/[0-9]/.test(password)) strength++;
    if (/[^a-zA-Z0-9]/.test(password)) strength++;

    if (strength <= 2) return 'weak';
    if (strength <= 3) return 'medium';
    return 'strong';
  }
}
```

### Date Utilities

```typescript
// shared/utils/date-utils.ts
export class DateUtils {
  static isToday(date: Date): boolean {
    const today = new Date();
    return date.toDateString() === today.toDateString();
  }

  static isSameDay(date1: Date, date2: Date): boolean {
    return date1.toDateString() === date2.toDateString();
  }

  static addDays(date: Date, days: number): Date {
    const result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
  }

  static differenceInDays(date1: Date, date2: Date): number {
    const diffTime = Math.abs(date2.getTime() - date1.getTime());
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  }

  static formatDate(date: Date, format: string = 'YYYY-MM-DD'): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return format
      .replace('YYYY', String(year))
      .replace('MM', month)
      .replace('DD', day)
      .replace('HH', hours)
      .replace('mm', minutes)
      .replace('ss', seconds);
  }
}
```

## Shared Error Handling

```typescript
// shared/errors/app-error.ts
export abstract class AppError extends Error {
  abstract readonly statusCode: number;
  abstract readonly isOperational: boolean;

  constructor(message: string) {
    super(message);
    Object.setPrototypeOf(this, new.target.prototype);
    Error.captureStackTrace(this, this.constructor);
  }
}

// shared/errors/validation-error.ts
export class ValidationError extends AppError {
  readonly statusCode = 400;
  readonly isOperational = true;

  constructor(
    public readonly field: string,
    message: string
  ) {
    super(message);
  }
}

// Export all errors
export * from './app-error';
export * from './validation-error';
// ... more errors
```

## Shared Logger

```typescript
// shared/logger/logger.ts
import winston from 'winston';

export class Logger {
  private logger: winston.Logger;

  constructor(moduleName: string) {
    this.logger = winston.createLogger({
      defaultMeta: { module: moduleName },
      format: winston.format.combine(winston.format.timestamp(), winston.format.json()),
      transports: [
        new winston.transports.Console(),
        new winston.transports.File({ filename: 'error.log', level: 'error' }),
        new winston.transports.File({ filename: 'combined.log' }),
      ],
    });
  }

  info(message: string, metadata?: Record<string, unknown>): void {
    this.logger.info(message, metadata);
  }

  error(message: string, error?: Error, metadata?: Record<string, unknown>): void {
    this.logger.error(message, { error: error?.stack, ...metadata });
  }

  warn(message: string, metadata?: Record<string, unknown>): void {
    this.logger.warn(message, metadata);
  }

  debug(message: string, metadata?: Record<string, unknown>): void {
    this.logger.debug(message, metadata);
  }
}
```

## Using Shared Libraries

### Installation in Projects

```bash
# Add to apps/my-app/package.json
{
  "dependencies": {
    "@project/types": "workspace:*",
    "@project/utils": "workspace:*",
    "@project/errors": "workspace:*"
  }
}
```

### Usage in Code

```typescript
// apps/my-app/src/services/user-service.ts
import { IUser, IUserCreateInput } from '@project/types';
import { Validators, StringUtils } from '@project/utils';
import { ValidationError, Logger } from '@project/errors';

export class UserService {
  private logger = new Logger('UserService');

  async createUser(input: IUserCreateInput): Promise<IUser> {
    // Validate input
    if (!Validators.validateEmail(input.email)) {
      throw new ValidationError('email', 'Invalid email format');
    }

    // Use utilities
    const normalizedName = StringUtils.pascalCase(input.name);

    this.logger.info('Creating user', { email: input.email });

    // Implementation...
  }
}
```

## Publishing Shared Packages

### Setup Monorepo

```json
// root package.json
{
  "workspaces": ["shared/*", "apps/*", "web/*", "tools/*", "desktop/*"]
}
```

### Package Configuration

```json
// shared/types/package.json
{
  "name": "@project/types",
  "version": "1.0.0",
  "main": "dist/index.js",
  "types": "dist/index.d.ts",
  "scripts": {
    "build": "tsc",
    "test": "vitest"
  }
}
```

---

**Last Updated**: 2026  
**Version**: 1.0
