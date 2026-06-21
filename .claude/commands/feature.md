# Implement a New Feature

Implement the following feature: $ARGUMENTS

## Context to gather first

- Which workspace? (`apps/web-frontend`, `apps/web-backend`, `apps/api`, `desktop/*`, `tools/*`, `shared/*`)
- Are there existing similar implementations to reference?
- Which shared types from `shared/types/src/index.ts` apply?

## Implementation steps

1. **Define types first** – Add or extend interfaces in `shared/types/src/` before writing logic
2. **Implement core logic** – Follow SOLID principles, keep functions small and single-purpose
3. **Error handling** – Use custom error classes (`ValidationError`, `NotFoundError`, etc.), handle all paths
4. **Write unit tests** – Vitest, AAA pattern, 80%+ coverage, mock external deps
5. **JSDoc documentation** – `@param`, `@returns`, `@throws`, include usage example
6. **Integration** – Wire into existing services/routes in the workspace

## Constraints

- TypeScript strict mode – no `any` types
- Naming: `camelCase` (vars/functions), `PascalCase` (types/classes), `UPPER_SNAKE_CASE` (constants)
- Logging via Pino (`apps/web-backend/src/utils/logger.ts`)
- Validation via Joi (backend) or Zod (frontend/shared)
- Tests in `*.test.ts` alongside source

## Deliverables

1. Implementation file in the correct workspace
2. Unit tests covering happy path + edge cases
3. JSDoc with examples
4. Conventional commit: `feat(<scope>): <description>`
