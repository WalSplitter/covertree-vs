---
name: Backend & Microservices Development
description: Best practices and patterns for building scalable backend services, APIs, microservices, and worker services
keywords:
  - backend
  - microservices
  - rest-api
  - express
  - nestjs
  - fastapi
  - typescript
  - nodejs
  - database
  - service-architecture
  - async-jobs
  - websockets
topics:
  - API Design & Implementation
  - Service Architecture
  - Database Design
  - Authentication & Authorization
  - Async Job Processing
  - Real-time Communication
  - Error Handling & Logging
applyTo:
  - /apps/**
relatedPrompts:
  - 01-feature-implementation.md
  - 04-api-design.md
  - 05-testing-strategy.md
  - 06-performance-optimization.md
relatedSkills:
  - error-handling.md
  - database-design.md
  - authentication.md
version: 1.0
---

# Backend & Microservices Development

Best practices for building production-grade backend services, APIs, and microservices with enterprise-level reliability.

## Service Architecture

### Express.js Service

```typescript
// src/index.ts
import express from 'express';
import cors from 'cors';
import helmet from 'helmet';
import { errorHandler } from './middleware/error-handler';
import { authMiddleware } from './middleware/auth';
import userRoutes from './api/routes/user-routes';

const app = express();

// Middleware
app.use(helmet());
app.use(cors({ origin: process.env.CORS_ORIGIN }));
app.use(express.json());

// Routes
app.use('/api/users', authMiddleware, userRoutes);

// Error handling
app.use(errorHandler);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
```

### NestJS Service

```typescript
// src/app.module.ts
import { Module } from '@nestjs/common';
import { ConfigModule } from '@nestjs/config';
import { TypeOrmModule } from '@nestjs/typeorm';
import { UserModule } from './modules/user/user.module';

@Module({
  imports: [
    ConfigModule.forRoot(),
    TypeOrmModule.forRoot({
      type: 'postgres',
      host: process.env.DB_HOST,
      port: parseInt(process.env.DB_PORT!),
      username: process.env.DB_USER,
      password: process.env.DB_PASSWORD,
      database: process.env.DB_NAME,
      synchronize: false,
      migrations: ['src/migrations/*.ts'],
    }),
    UserModule,
  ],
})
export class AppModule {}

// src/modules/user/user.controller.ts
import { Controller, Get, Post, Body, Param } from '@nestjs/common';
import { UserService } from './user.service';

@Controller('users')
export class UserController {
  constructor(private readonly userService: UserService) {}

  @Get(':id')
  async getUser(@Param('id') id: string) {
    return this.userService.getUserById(id);
  }

  @Post()
  async createUser(@Body() createUserDto: CreateUserDto) {
    return this.userService.createUser(createUserDto);
  }
}
```

## API Design Patterns

### RESTful Endpoints

```typescript
// Organized route structure
interface IUserRepository {
  findById(id: string): Promise<IUser | null>;
  findAll(skip: number, take: number): Promise<IPaginatedResult<IUser>>;
  create(data: IUserCreateInput): Promise<IUser>;
  update(id: string, data: Partial<IUser>): Promise<IUser>;
  delete(id: string): Promise<void>;
}

// Service layer
export class UserService {
  constructor(private repository: IUserRepository) {}

  async getUserById(id: string): Promise<IUser> {
    const user = await this.repository.findById(id);
    if (!user) throw new NotFoundError('User', id);
    return user;
  }

  async listUsers(skip = 0, take = 10): Promise<IPaginatedResult<IUser>> {
    if (take > 100) throw new ValidationError('take', 'Maximum 100 items');
    return this.repository.findAll(skip, take);
  }

  async createUser(input: IUserCreateInput): Promise<IUser> {
    // Validation
    if (!isValidEmail(input.email)) {
      throw new ValidationError('email', 'Invalid email');
    }

    // Check uniqueness
    const existing = await this.repository.findByEmail(input.email);
    if (existing) throw new ConflictError('Email already registered');

    // Hash password
    const hashedPassword = await hashPassword(input.password);

    return this.repository.create({
      ...input,
      password: hashedPassword,
    });
  }
}

// Controller/Route handler
export async function handleGetUser(req: Request, res: Response) {
  const service = req.app.get(UserService);
  const user = await service.getUserById(req.params.id);
  res.json(user);
}

export async function handleListUsers(req: Request, res: Response) {
  const service = req.app.get(UserService);
  const skip = parseInt(req.query.skip as string) || 0;
  const take = parseInt(req.query.take as string) || 10;
  const result = await service.listUsers(skip, take);
  res.json(result);
}

export async function handleCreateUser(req: Request, res: Response) {
  const service = req.app.get(UserService);
  const user = await service.createUser(req.body);
  res.status(201).json(user);
}
```

## Database Patterns

### Repository Pattern

```typescript
// src/repositories/user-repository.ts
export class UserRepository implements IUserRepository {
  constructor(private db: Database) {}

  async findById(id: string): Promise<IUser | null> {
    return this.db
      .query<IUser>('SELECT * FROM users WHERE id = $1', [id])
      .then((rows) => rows[0] || null);
  }

  async findByEmail(email: string): Promise<IUser | null> {
    return this.db
      .query<IUser>('SELECT * FROM users WHERE email = $1', [email])
      .then((rows) => rows[0] || null);
  }

  async findAll(skip: number, take: number): Promise<IPaginatedResult<IUser>> {
    const [users, countResult] = await Promise.all([
      this.db.query<IUser>('SELECT * FROM users ORDER BY created_at DESC LIMIT $1 OFFSET $2', [
        take,
        skip,
      ]),
      this.db.query<{ count: string }>('SELECT COUNT(*) as count FROM users'),
    ]);

    const total = parseInt(countResult[0]?.count || '0');

    return {
      items: users,
      total,
      hasMore: skip + take < total,
      page: Math.floor(skip / take) + 1,
      pageSize: take,
    };
  }

  async create(data: IUserCreateInput): Promise<IUser> {
    const result = await this.db.query<IUser>(
      `INSERT INTO users (email, name, password, created_at)
       VALUES ($1, $2, $3, NOW())
       RETURNING *`,
      [data.email, data.name, data.password]
    );
    return result[0];
  }

  async update(id: string, data: Partial<IUser>): Promise<IUser> {
    const updateFields = Object.keys(data)
      .filter((k) => data[k as keyof IUser] !== undefined)
      .map((k, i) => `${k} = $${i + 2}`)
      .join(', ');

    const values = Object.values(data).filter((v) => v !== undefined);

    const result = await this.db.query<IUser>(
      `UPDATE users SET ${updateFields}, updated_at = NOW()
       WHERE id = $1
       RETURNING *`,
      [id, ...values]
    );
    return result[0];
  }

  async delete(id: string): Promise<void> {
    await this.db.query('DELETE FROM users WHERE id = $1', [id]);
  }
}
```

### Transaction Management

```typescript
async function transferFunds(fromUserId: string, toUserId: string, amount: number) {
  const client = await db.pool.connect();
  try {
    await client.query('BEGIN');

    // Deduct from source
    await client.query('UPDATE accounts SET balance = balance - $1 WHERE user_id = $2', [
      amount,
      fromUserId,
    ]);

    // Add to destination
    await client.query('UPDATE accounts SET balance = balance + $1 WHERE user_id = $2', [
      amount,
      toUserId,
    ]);

    // Record transaction
    await client.query('INSERT INTO transactions (from_id, to_id, amount) VALUES ($1, $2, $3)', [
      fromUserId,
      toUserId,
      amount,
    ]);

    await client.query('COMMIT');
  } catch (error) {
    await client.query('ROLLBACK');
    throw error;
  } finally {
    client.release();
  }
}
```

## Authentication & Authorization

### JWT Implementation

```typescript
// src/services/auth-service.ts
export class AuthService {
  async login(email: string, password: string): Promise<IAuthToken> {
    const user = await this.userRepository.findByEmail(email);
    if (!user) throw new AuthenticationError('Invalid credentials');

    const isValid = await verifyPassword(password, user.password);
    if (!isValid) throw new AuthenticationError('Invalid credentials');

    const token = this.generateToken(user);
    const refreshToken = this.generateRefreshToken(user);

    return { token, refreshToken, user };
  }

  private generateToken(user: IUser): string {
    return jwt.sign(
      {
        userId: user.id,
        email: user.email,
        role: user.role,
      },
      process.env.JWT_SECRET!,
      { expiresIn: '24h' }
    );
  }

  verifyToken(token: string): ITokenPayload {
    try {
      return jwt.verify(token, process.env.JWT_SECRET!) as ITokenPayload;
    } catch {
      throw new AuthenticationError('Invalid or expired token');
    }
  }
}

// Middleware
export function authMiddleware(req: Request, res: Response, next: NextFunction) {
  const authHeader = req.headers.authorization;
  if (!authHeader?.startsWith('Bearer ')) {
    return res.status(401).json({ error: 'No token provided' });
  }

  const token = authHeader.substring(7);
  try {
    const payload = authService.verifyToken(token);
    (req as any).user = payload;
    next();
  } catch (error) {
    res.status(401).json({ error: 'Invalid token' });
  }
}

// Role-based access
export function authorize(...allowedRoles: string[]) {
  return (req: Request, res: Response, next: NextFunction) => {
    if (!allowedRoles.includes((req as any).user.role)) {
      return res.status(403).json({ error: 'Forbidden' });
    }
    next();
  };
}
```

## Async Job Processing

### Bull Queue Pattern

```typescript
// src/queues/email-queue.ts
import { Queue, Worker } from 'bullmq';
import Redis from 'ioredis';

const redis = new Redis({
  host: process.env.REDIS_HOST,
  port: parseInt(process.env.REDIS_PORT!),
});

const emailQueue = new Queue('emails', { connection: redis });

// Add job
export async function sendWelcomeEmail(userId: string, email: string) {
  await emailQueue.add(
    'welcome',
    { userId, email },
    {
      attempts: 3,
      backoff: { type: 'exponential', delay: 2000 },
    }
  );
}

// Process jobs
const emailWorker = new Worker(
  'emails',
  async (job) => {
    try {
      await emailService.send({
        to: job.data.email,
        subject: 'Welcome!',
        template: 'welcome',
      });
      job.log('Email sent successfully');
    } catch (error) {
      logger.error('Email failed:', error);
      throw error; // Retry
    }
  },
  { connection: redis, concurrency: 5 }
);

// Monitor
emailWorker.on('completed', (job) => {
  logger.info(`Job ${job.id} completed`);
});

emailWorker.on('failed', (job, err) => {
  logger.error(`Job ${job?.id} failed:`, err);
});
```

## Real-time Communication

### WebSocket Service

```typescript
// src/gateway/events-gateway.ts
import { Server, Socket } from 'socket.io';

export class EventsGateway {
  private io: Server;

  constructor(server: http.Server) {
    this.io = new Server(server, {
      cors: { origin: process.env.CORS_ORIGIN },
    });

    this.setupMiddleware();
    this.setupHandlers();
  }

  private setupMiddleware() {
    this.io.use(async (socket, next) => {
      const token = socket.handshake.auth.token;
      try {
        const payload = authService.verifyToken(token);
        socket.data.userId = payload.userId;
        next();
      } catch {
        next(new Error('Authentication error'));
      }
    });
  }

  private setupHandlers() {
    this.io.on('connection', (socket: Socket) => {
      const userId = socket.data.userId;

      // Join user room
      socket.join(`user:${userId}`);

      socket.on('message', async (data) => {
        try {
          const message = await messageService.create({
            ...data,
            userId,
          });
          this.io.to(`user:${data.recipientId}`).emit('message:new', message);
        } catch (error) {
          socket.emit('error', error.message);
        }
      });

      socket.on('disconnect', () => {
        logger.info(`User ${userId} disconnected`);
      });
    });
  }

  // Emit to user
  emitToUser(userId: string, event: string, data: unknown) {
    this.io.to(`user:${userId}`).emit(event, data);
  }

  // Broadcast to all
  broadcast(event: string, data: unknown) {
    this.io.emit(event, data);
  }
}
```

## Error Handling & Logging

### Global Error Handler

```typescript
// src/middleware/error-handler.ts
export function errorHandler(error: Error, req: Request, res: Response, next: NextFunction) {
  const statusCode = getStatusCode(error);
  const message = getMessage(error);

  logger.error({
    error: error.message,
    stack: error.stack,
    url: req.url,
    method: req.method,
    statusCode,
  });

  res.status(statusCode).json({
    error: {
      code: getErrorCode(error),
      message,
      timestamp: new Date().toISOString(),
      ...(process.env.NODE_ENV === 'development' && {
        stack: error.stack,
      }),
    },
  });
}

function getStatusCode(error: Error): number {
  if (error instanceof ValidationError) return 400;
  if (error instanceof AuthenticationError) return 401;
  if (error instanceof AuthorizationError) return 403;
  if (error instanceof NotFoundError) return 404;
  if (error instanceof ConflictError) return 409;
  return 500;
}

function getMessage(error: Error): string {
  if (error instanceof AppError) return error.message;
  if (process.env.NODE_ENV === 'production') {
    return 'Internal server error';
  }
  return error.message;
}
```

### Structured Logging

```typescript
// src/utils/logger.ts
import winston from 'winston';

export const logger = winston.createLogger({
  level: process.env.LOG_LEVEL || 'info',
  format: winston.format.combine(
    winston.format.timestamp(),
    winston.format.errors({ stack: true }),
    winston.format.json()
  ),
  defaultMeta: { service: 'user-service' },
  transports: [
    new winston.transports.File({ filename: 'logs/error.log', level: 'error' }),
    new winston.transports.File({ filename: 'logs/combined.log' }),
    ...(process.env.NODE_ENV !== 'production' && [
      new winston.transports.Console({
        format: winston.format.combine(winston.format.colorize(), winston.format.simple()),
      }),
    ]),
  ],
});
```

## Testing Backend Services

```typescript
// tests/user-service.test.ts
describe('UserService', () => {
  let userService: UserService;
  let userRepository: jest.Mocked<IUserRepository>;

  beforeEach(() => {
    userRepository = {
      findById: jest.fn(),
      create: jest.fn(),
      // ... other methods
    } as any;

    userService = new UserService(userRepository);
  });

  describe('createUser', () => {
    it('should create user with hashed password', async () => {
      const input = {
        email: 'test@example.com',
        name: 'Test User',
        password: 'password123',
      };

      userRepository.findByEmail.mockResolvedValue(null);
      userRepository.create.mockResolvedValue({
        id: 'user-123',
        ...input,
      });

      const result = await userService.createUser(input);

      expect(result.id).toBe('user-123');
      expect(userRepository.create).toHaveBeenCalledWith(
        expect.objectContaining({ email: input.email })
      );
    });

    it('should throw for duplicate email', async () => {
      userRepository.findByEmail.mockResolvedValue({
        id: 'existing',
        email: 'test@example.com',
      } as IUser);

      await expect(
        userService.createUser({
          email: 'test@example.com',
          name: 'Test',
          password: 'pwd',
        })
      ).rejects.toThrow(ConflictError);
    });
  });
});
```

## Key Takeaways

✅ **Service Layer**: Separate business logic from HTTP/transport
✅ **Repository Pattern**: Abstract data access for testability
✅ **Error Handling**: Consistent error format with appropriate status codes
✅ **Authentication**: Secure token-based auth with proper middleware
✅ **Async Jobs**: Handle long-running tasks without blocking requests
✅ **Logging**: Structured logging for debugging and monitoring
✅ **Testing**: Isolated unit tests with mocked dependencies

---

**Related Resources**:

- Prompts: [Feature Implementation](../prompts/01-feature-implementation.md), [API Design](../prompts/04-api-design.md)
- Skills: [Error Handling](error-handling.md), [Database Design](database-design.md)
- Standards: See [copilot-instructions.md](../copilot-instructions.md)
