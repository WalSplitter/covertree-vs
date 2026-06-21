# Prompt: Performance Optimization - Base Project Template

Use this prompt template when optimizing performance with GitHub Copilot.

## Template

```
Performance Issue:
Area: [FRONTEND/BACKEND/DATABASE/NETWORK]
Workspace: [WEB/APPS/DESKTOP/TOOLS]
Symptom: [OBSERVABLE_PROBLEM]
Impact: [HOW_IT_AFFECTS_USERS]
Current metric: [BASELINE_MEASUREMENT]
Tools: Vite dev tools (frontend), Node.js inspector (backend), npm run dev

Analysis:
- Bottleneck identified: [WHERE_TIME_IS_SPENT]
- Root cause: [WHY_IT'S_SLOW]
- Affected operations: [FUNCTIONS/QUERIES/REQUESTS]
- User impact: [LATENCY/MEMORY/CPU]

Optimization Strategy:
- Approach: [CACHING via Redis/PAGINATION/LAZY_LOADING/DATABASE_INDEXING]
- Trade-offs: [MEMORY_VS_SPEED, EVENTUAL_CONSISTENCY, etc.]
- Complexity: [O(n) → O(log n), N+1 → JOIN query, etc.]
- Tools available: Redis (docker-compose.yml), PostgreSQL query analysis, Vite bundle analysis

Target Metrics:
- Latency: [TARGET_TIME]
- Memory: [TARGET_USAGE]
- CPU: [TARGET_USAGE]
- Success criteria: [MEASUREMENT]

Deliverables:
1. Optimized implementation
2. Before/after performance comparison (metrics with units)
3. Monitoring setup (optional: logging with Pino, GitHub Actions)
4. Documentation of optimization strategy
5. Regression tests (vitest) to prevent regressions
6. Commit: perf(scope): description of performance improvements

Validate:
\`\`\`bash
npm run build --workspaces    # Check bundle size
npm run test --workspaces     # Ensure no regressions
make docker-up               # Test with services
\`\`\`
```

## Example Usage

```
Performance Issue:
Area: Backend - API endpoint
Symptom: /api/users/:id/posts responds in 3+ seconds
Impact: User experience degraded, potential timeouts
Current metric: P95 latency: 3.2s, Database queries: 50+

Analysis:
- Bottleneck identified: N+1 query problem in post fetching
- Root cause: Loop fetching comments for each post individually
- Affected operations: getUserWithPosts() function
- User impact: Linear latency with post count

Optimization Strategy:
- Approach: Batch load comments using JOIN query, add Redis caching
- Trade-offs: Slight memory increase for cache, eventual consistency
- Complexity: O(n+m) → O(1) for cached results

Target Metrics:
- Latency: < 500ms P95 (6x improvement)
- Memory: +50MB for cache (acceptable)
- CPU: Reduced by 40% from fewer queries
- Success criteria: < 500ms for 100 posts

Deliverables:
1. Optimized query with eager loading
2. Redis cache layer
3. Performance benchmark (3.2s → 300ms)
4. Cache invalidation strategy docs
5. Load tests validating improvements
```

## Optimization Techniques

### Database Query Optimization

```typescript
// Before: N+1 queries
async function getUserWithPosts(userId: string) {
  const user = await db.users.findOne({ id: userId });
  const posts = await db.posts.find({ userId });

  return {
    ...user,
    posts: await Promise.all(
      posts.map(
        (post) => db.comments.find({ postId: post.id }) // N queries for comments
      )
    ),
  };
}

// After: Single query with joins
async function getUserWithPosts(userId: string) {
  return db.users.findOne({ id: userId }).populate({
    path: 'posts',
    populate: { path: 'comments' },
  });
}
```

### Caching Strategy

```typescript
class CachedUserRepository {
  constructor(
    private repo: UserRepository,
    private cache: ICache<IUser>
  ) {}

  async getUserById(id: string): Promise<IUser> {
    // Check cache first
    const cached = await this.cache.get(`user:${id}`);
    if (cached) return cached;

    // Load from database
    const user = await this.repo.findById(id);
    if (user) {
      // Cache with 15-minute TTL
      await this.cache.set(`user:${id}`, user, 15 * 60 * 1000);
    }

    return user;
  }

  async updateUser(id: string, data: Partial<IUser>): Promise<IUser> {
    const updated = await this.repo.update(id, data);
    // Invalidate cache
    await this.cache.delete(`user:${id}`);
    return updated;
  }
}
```

### Pagination for Large Datasets

```typescript
interface IPaginationParams {
  skip: number;
  take: number;
  sortBy?: string;
  sortOrder?: 'asc' | 'desc';
}

interface IPaginatedResult<T> {
  items: T[];
  total: number;
  hasMore: boolean;
  pageInfo: {
    currentPage: number;
    totalPages: number;
    pageSize: number;
  };
}

async function getUsers(params: IPaginationParams): Promise<IPaginatedResult<IUser>> {
  const { skip, take, sortBy = 'createdAt', sortOrder = 'desc' } = params;

  const [items, total] = await Promise.all([
    db.users
      .find()
      .skip(skip)
      .limit(take)
      .sort({ [sortBy]: sortOrder === 'desc' ? -1 : 1 }),
    db.users.countDocuments(),
  ]);

  const pageSize = take;
  const currentPage = Math.floor(skip / take) + 1;
  const totalPages = Math.ceil(total / pageSize);

  return {
    items,
    total,
    hasMore: skip + take < total,
    pageInfo: {
      currentPage,
      totalPages,
      pageSize,
    },
  };
}
```

### Frontend Lazy Loading

```typescript
// React example
import { lazy, Suspense } from 'react';

const HeavyComponent = lazy(() =>
  import('./HeavyComponent')
);

function App() {
  return (
    <Suspense fallback={<LoadingSpinner />}>
      <HeavyComponent />
    </Suspense>
  );
}
```

### Debouncing User Input

```typescript
function useDebounce<T>(value: T, delayMs: number = 500): T {
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedValue(value);
    }, delayMs);

    return () => clearTimeout(handler);
  }, [value, delayMs]);

  return debouncedValue;
}

// Usage: Search with debounced API call
function SearchUsers() {
  const [query, setQuery] = useState('');
  const debouncedQuery = useDebounce(query, 300);
  const [results, setResults] = useState([]);

  useEffect(() => {
    if (debouncedQuery) {
      searchUsers(debouncedQuery).then(setResults);
    }
  }, [debouncedQuery]);

  return (
    <>
      <input value={query} onChange={e => setQuery(e.target.value)} />
      <Results items={results} />
    </>
  );
}
```

## Monitoring & Observability

```typescript
// Instrument critical paths
async function monitoredGetUser(id: string): Promise<IUser> {
  const startTime = performance.now();
  const timer = metrics.startTimer('getUserById');

  try {
    const user = await userService.getUserById(id);
    metrics.recordDuration('getUserById', performance.now() - startTime);
    return user;
  } catch (error) {
    metrics.incrementCounter('getUserById.errors');
    throw error;
  } finally {
    timer.end();
  }
}
```

## Profiling & Benchmarking

```typescript
// Node.js performance measurement
import { performance } from 'perf_hooks';

function benchmark(label: string, fn: () => void, iterations: number = 1000) {
  const start = performance.now();

  for (let i = 0; i < iterations; i++) {
    fn();
  }

  const duration = performance.now() - start;
  console.log(`${label}: ${(duration / iterations).toFixed(2)}ms per iteration`);
}
```
