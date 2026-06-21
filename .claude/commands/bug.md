# Debug and Fix a Bug

Investigate and fix the following issue: $ARGUMENTS

## Diagnostic steps

```bash
npm run type-check --workspaces      # Catch type errors
npm run lint -w <workspace>          # Check linting
npm run test:watch -w <workspace>    # Run tests in watch mode
```

## Investigation template

Provide as much of the following as possible:

```
File:             [path/to/file.ts]
Workspace:        [web | apps/* | desktop/* | tools/* | shared/*]
Current behavior: [what actually happens]
Expected:         [what should happen]
Error message:    [exact error or log output]
Likely cause:     [your hypothesis]
Recent changes:   [if any]
```

## Common root causes

**Race condition / duplicate data** → Use DB transactions, pessimistic locking (`SELECT FOR UPDATE`), or unique constraints

**Type mismatch** → Check TypeScript strict mode, add type guards for runtime validation

**Performance degradation** → Profile with Node.js inspector, look for N+1 queries or missing indexes

**Memory leak** → Check for unremoved event listeners, unresolved promises, circular references

## Deliverables

1. Corrected implementation with root-cause comment explaining _why_, not just _what_
2. Regression test covering the exact bug scenario (Vitest)
3. Prevention note (linting rule, type constraint, or code pattern to avoid recurrence)
4. Conventional commit: `fix(<scope>): <description>`
