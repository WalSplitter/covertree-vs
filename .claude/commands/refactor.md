# Refactor Code

Refactor the following: $ARGUMENTS

## Before starting

- Read the existing code carefully before suggesting changes
- Identify which SOLID principles are violated
- Check test coverage: `npm run test:coverage -w <workspace>`
- Confirm public API compatibility requirements

## Common refactoring goals

**Extract Method** – Break large functions into focused, single-purpose functions (<100 lines each)

**Extract Class** – Split god objects into focused classes (`UserValidator`, `UserTransformer`, `UserRepository`, `UserService`)

**Remove `any` types** – Replace with strict interfaces, use discriminated unions or generics

**Apply patterns** – Observer for loose coupling, Factory for object creation, Repository for data access

## Quality targets

| Metric                         | Target            |
| ------------------------------ | ----------------- |
| Cyclomatic complexity          | < 10 per function |
| Function length                | < 100 lines       |
| Parameters per function        | ≤ 3               |
| Test coverage (business logic) | ≥ 80%             |
| Code duplication               | < 5%              |

## Deliverables

1. Refactored implementation – maintain backward compatibility unless explicitly changing the API
2. Updated tests covering the new structure
3. Migration notes if any consumer code needs updating
4. Conventional commit: `refactor(<scope>): <description>`
