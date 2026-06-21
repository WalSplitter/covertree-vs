# Applications & Services

Dedicated directory for backend applications, microservices, and server-side projects.

## Project Structure

```
apps/
├── api-service/               # RESTful API service
│   ├── src/
│   │   ├── api/
│   │   │   ├── routes/       # API endpoints
│   │   │   ├── controllers/  # Request handlers
│   │   │   └── middleware/   # Express/Koa middleware
│   │   ├── domain/
│   │   │   ├── models/       # Business models
│   │   │   ├── services/     # Business logic
│   │   │   └── repositories/ # Data access
│   │   ├── infrastructure/
│   │   │   ├── database/     # DB configuration
│   │   │   ├── cache/        # Caching layer
│   │   │   └── external/     # Third-party integrations
│   │   ├── config/           # Configuration
│   │   ├── types/            # TypeScript types
│   │   ├── utils/            # Utilities
│   │   └── index.ts          # Entry point
│   ├── tests/
│   ├── .env.example
│   ├── package.json
│   └── README.md
│
├── worker-service/            # Background job processing
│   ├── src/
│   │   ├── jobs/             # Job definitions
│   │   ├── queues/           # Queue setup (Bull, BullMQ)
│   │   ├── handlers/         # Job handlers
│   │   ├── config/
│   │   ├── types/
│   │   └── index.ts
│   ├── tests/
│   └── package.json
│
├── websocket-service/         # Real-time communication
│   ├── src/
│   │   ├── gateway/          # Socket.IO setup
│   │   ├── handlers/         # Event handlers
│   │   ├── rooms/            # Room management
│   │   ├── types/
│   │   └── index.ts
│   ├── tests/
│   └── package.json
│
└── cli-tool/                  # Command-line application
    ├── src/
    │   ├── commands/         # CLI commands
    │   ├── services/         # Business logic
    │   ├── config/
    │   ├── types/
    │   └── cli.ts            # Entry point
    ├── tests/
    └── package.json
```

## Technology Recommendations

### Backend Framework

#### Express.js

```bash
npm install express express-async-errors dotenv cors helmet
npm install -D @types/express typescript

# Best for: RESTful APIs, lightweight services
```

#### NestJS

```bash
npm install -g @nestjs/cli
nest new apps/api-service

# Best for: Large-scale applications, microservices
# Features: Built-in DI, testing, validation
```

#### FastAPI (Python)

```bash
pip install fastapi uvicorn sqlalchemy pydantic

# Best for: High performance APIs, data validation
# Features: Automatic docs, async support
```

#### ASP.NET Core (C#)

```bash
dotnet new webapi -n apps/ApiService
# Best for: Enterprise applications, .NET ecosystem
```

### Database & ORM

#### TypeScript/Node.js

- **ORM**: TypeORM, Prisma, Sequelize
- **Query Builder**: Knex.js, QueryBuilder
- **Database**: PostgreSQL, MongoDB, MySQL

#### Python

- **ORM**: SQLAlchemy, Django ORM
- **Database**: PostgreSQL, MySQL, SQLite

#### C#

- **ORM**: Entity Framework Core
- **Database**: SQL Server, PostgreSQL, MySQL

### Message Queues & Background Jobs

```typescript
// BullMQ for job processing
import { Queue, Worker } from 'bullmq';
import Redis from 'ioredis';

const redisConnection = new Redis();
const emailQueue = new Queue('emails', { connection: redisConnection });

// Add job
await emailQueue.add(
  'send-email',
  {
    to: 'user@example.com',
    subject: 'Welcome!',
  },
  { delay: 5000 }
);

// Process job
const emailWorker = new Worker(
  'emails',
  async (job) => {
    await sendEmail(job.data);
  },
  { connection: redisConnection }
);
```

### Real-time Communication

```typescript
// Socket.IO for WebSocket connections
import { Server } from 'socket.io';
import http from 'http';

const httpServer = http.createServer(app);
const io = new Server(httpServer);

io.on('connection', (socket) => {
  socket.on('message', (data) => {
    io.emit('broadcast', data);
  });

  socket.on('disconnect', () => {
    console.log('User disconnected');
  });
});

httpServer.listen(3000);
```

## Common Patterns

### Authentication & Authorization

```typescript
// JWT middleware
import jwt from 'jsonwebtoken';

interface IAuthRequest extends Request {
  user?: { userId: string; role: string };
}

export const authMiddleware = (req: IAuthRequest, res: Response, next: NextFunction) => {
  const token = req.headers.authorization?.split(' ')[1];

  if (!token) {
    return res.status(401).json({ error: 'No token provided' });
  }

  try {
    const decoded = jwt.verify(token, process.env.JWT_SECRET!) as any;
    req.user = decoded;
    next();
  } catch (error) {
    res.status(401).json({ error: 'Invalid token' });
  }
};

// Role-based access control
const authorize = (allowedRoles: string[]) => {
  return (req: IAuthRequest, res: Response, next: NextFunction) => {
    if (!req.user || !allowedRoles.includes(req.user.role)) {
      return res.status(403).json({ error: 'Forbidden' });
    }
    next();
  };
};
```

### Error Handling

```typescript
// Global error handler
app.use((error: Error, req: Request, res: Response, next: NextFunction) => {
  if (error instanceof ValidationError) {
    return res.status(400).json({
      error: { type: 'VALIDATION_ERROR', message: error.message },
    });
  }

  if (error instanceof NotFoundError) {
    return res.status(404).json({
      error: { type: 'NOT_FOUND', message: error.message },
    });
  }

  logger.error('Unhandled error:', error);
  res.status(500).json({
    error: { type: 'INTERNAL_ERROR', message: 'Internal server error' },
  });
});
```

### Database Transactions

```typescript
// TypeORM example
async function transferFunds(fromId: string, toId: string, amount: number) {
  const queryRunner = dataSource.createQueryRunner();
  await queryRunner.connect();
  await queryRunner.startTransaction();

  try {
    await queryRunner.manager.decrement(User, { id: fromId }, 'balance', amount);
    await queryRunner.manager.increment(User, { id: toId }, 'balance', amount);

    await queryRunner.commitTransaction();
  } catch (error) {
    await queryRunner.rollbackTransaction();
    throw error;
  } finally {
    await queryRunner.release();
  }
}
```

### Logging & Monitoring

```typescript
import winston from 'winston';

const logger = winston.createLogger({
  level: process.env.LOG_LEVEL || 'info',
  format: winston.format.json(),
  transports: [
    new winston.transports.Console(),
    new winston.transports.File({ filename: 'error.log', level: 'error' }),
    new winston.transports.File({ filename: 'combined.log' }),
  ],
});

// Usage
logger.info('User registered', { userId: user.id });
logger.error('Database connection failed', { error });
```

## Environment Configuration

```bash
# .env.example
NODE_ENV=development
PORT=3000
DATABASE_URL=postgresql://user:password@localhost:5432/dbname
REDIS_URL=redis://localhost:6379
JWT_SECRET=your-secret-key
LOG_LEVEL=debug
```

## Getting Started

1. Choose your application type (API, Worker, WebSocket, CLI)
2. Choose your framework (Express, NestJS, FastAPI, ASP.NET)
3. Initialize project:
   ```bash
   mkdir apps/my-service
   cd apps/my-service
   npm init -y
   npm install express dotenv typescript
   ```
4. Review [.github/copilot-instructions.md](../.github/copilot-instructions.md)
5. Start development:
   ```bash
   npm run dev
   ```

## Testing

```bash
npm run test              # Run all tests
npm run test:unit        # Unit tests only
npm run test:integration # Integration tests
npm run test:coverage    # Coverage report
```

## Deployment

### Local Development

```bash
npm run dev              # Start with hot reload
```

### Production

```bash
npm run build           # Compile TypeScript
npm run start           # Start production server
```

### Docker

```bash
docker build -t my-service .
docker run -p 3000:3000 my-service
```

### Cloud Platforms

- **Heroku**: `git push heroku main`
- **AWS**: ECS, Lambda, EC2
- **DigitalOcean**: App Platform, Droplets
- **Render**: Connect GitHub repository
- **Railway**: Automatic deployment

---

**Last Updated**: 2026  
**Version**: 1.0
