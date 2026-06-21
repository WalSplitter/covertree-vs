# Optimize Performance

Optimize the following performance issue: $ARGUMENTS

## Diagnosis first

Before optimizing, measure the baseline:

```bash
npm run dev -w <workspace>          # Run with Node.js inspector
npm run build --workspaces          # Check bundle size
make docker-up                      # Start Postgres + Redis for realistic testing
```

## Issue template

```
Area:            [frontend | backend | database | network]
Workspace:       [web | apps/* | desktop/* | tools/*]
Symptom:         [e.g. /api/users/:id responds in 3s+]
Current metric:  [P95 latency, memory MB, CPU %, bundle KB]
Target metric:   [e.g. < 500ms P95]
Root cause:      [e.g. N+1 queries, no caching, large bundle]
```

## Common optimizations

**N+1 queries** → Replace loops with JOIN queries or batch loading

```typescript
// Before: N+1
const posts = await db.posts.find({ userId });
const withComments = await Promise.all(posts.map((p) => db.comments.find({ postId: p.id })));
// After: single query with populate/join
const posts = await db.posts.find({ userId }).populate('comments');
```

**Missing cache** → Add Redis layer with TTL and cache invalidation on write

**Large bundle** → React `lazy()` + `Suspense`, dynamic imports, tree-shaking audit

**Unthrottled events** → Debounce user input (300ms), throttle scroll/resize handlers

**Slow pagination** → Use cursor-based or offset pagination, add DB indexes

## Validation

```bash
npm run test --workspaces     # No regressions
npm run build --workspaces    # Bundle size check
```

## Deliverables

1. Optimized implementation
2. Before/after metrics (concrete numbers with units)
3. Regression tests to prevent performance degradation
4. Brief note on trade-offs (memory vs. speed, eventual consistency, etc.)
5. Conventional commit: `perf(<scope>): <description>`
