# Write Tests

Write tests for the following: $ARGUMENTS

## Test context to confirm

- Workspace: `web` | `apps/*` | `desktop/*` | `tools/*` | `shared/*`
- Test type: unit | integration | both
- What to mock: database (PostgreSQL), Redis, external APIs, file system

## Run tests

```bash
npm run test -w <workspace>              # Run once
npm run test:watch -w <workspace>        # Watch mode
npm run test:coverage -w <workspace>     # With coverage
```

## Test file conventions

- Location: `*.test.ts` alongside source file
- Framework: **Vitest** with `vi.mock()` for mocking
- Environment: `jsdom` (frontend) or `node` (backend/shared)
- Coverage target: **80%+** for business logic

## AAA Pattern (required)

```typescript
describe('UserService', () => {
  describe('getUserProfile', () => {
    it('should return user profile when user exists', async () => {
      // Arrange
      mockRepository.findById.mockResolvedValue(expectedUser);

      // Act
      const result = await userService.getUserProfile('user-123');

      // Assert
      expect(result).toEqual(expectedUser);
      expect(mockRepository.findById).toHaveBeenCalledWith('user-123');
    });
  });
});
```

## Required test scenarios

For every function, cover:

1. **Happy path** – expected input, expected output
2. **Edge cases** – empty arrays, null/undefined, boundary values
3. **Error scenarios** – throws correct error type with correct message
4. **Async behavior** – resolved values, rejected promises, timeouts

## Naming convention

```typescript
it('should <expected behavior> when <condition>', () => { ... });
```

## Coverage targets

```
Business logic (services, validators):  80%+
API handlers / controllers:             70%+
Infrastructure / repositories:          50%+
Auto-generated code:                    skip
```

## Deliverables

1. Complete test file with all scenarios
2. Mock/fixture setup (reusable across tests)
3. Coverage passing 80%+ for the module
4. Conventional commit: `test(<scope>): <description>`
