# Prompt: Refactoring & Code Quality - Base Project Template

Use this prompt template when improving code quality and refactoring with GitHub Copilot.

## 📋 Before Refactoring

Check:

- [SOLID Principles](./docs/README.md#learning-resources) - Single Responsibility, Open/Closed
- [Code Standards](./.github/copilot-instructions.md) - Naming, organization, patterns
- Existing patterns in workspace for consistency

## Template

```
Current State:
File: [FILE_PATH in WORKSPACE]
Current code: [CODE_SNIPPET]
Current issues:
- [ISSUE_1: violates SOLID, hard to test, etc.]
- [ISSUE_2]
- [ISSUE_3]

Goal:
Refactor to: [TARGET_STATE]
Following: SOLID principles, code standards, monorepo best practices
Constraints: [BACKWARD_COMPATIBILITY, PUBLIC_API_STABILITY]

Quality Improvements:
- Readability: Clear function names (camelCase), small focused functions
- Maintainability: Follow SOLID, reduce coupling, improve organization
- Performance: Measure before/after with profiling
- Type Safety: Remove 'any' types, add strict interfaces from shared/types/
- Test Coverage: Increase from X% to 80%+ (run npm run test:coverage)

Deliverables:
1. Refactored implementation maintaining API compatibility
2. Updated tests (vitest) with new structure coverage
3. Migration guide (if breaking changes for workspace consumers)
4. Performance comparison and benchmarks
5. Commit: refactor(scope): description of improvements
```

## Example Usage

```
Current State:
File: src/utils/data-processor.ts
Current code: Large function with nested loops, multiple responsibilities
Current issues:
- 400+ lines, hard to understand
- Mixed data validation and transformation logic
- No type safety (uses 'any' in 5 places)
- Difficult to unit test
- No error handling

Goal:
Refactor to: Service-oriented architecture with clear separation of concerns
Following: SOLID principles, particularly SRP and DIP
Constraints: Public API must remain backward compatible

Quality Improvements:
- Readability: Split into focused, single-purpose functions
- Maintainability: Clear dependencies, easier to modify
- Performance: Remove unnecessary loops, add memoization
- Type Safety: Remove all 'any' types, add strict interfaces
- Test Coverage: Increase from 30% to 80%+

Deliverables:
1. DataProcessorService with clearly named methods
2. Updated tests with comprehensive coverage
3. Migration guide for consumers
4. Performance metrics showing improvements
```

## Refactoring Strategies

### Extract Method

```typescript
// Before: Large function
function processUserData(data: unknown) {
  // 50 lines of validation
  // 30 lines of transformation
  // 20 lines of persistence
}

// After: Clear responsibilities
function validateUserData(data: unknown): IUserData {
  /* ... */
}
function transformUserData(data: IUserData): IUserDataDTO {
  /* ... */
}
async function persistUserData(data: IUserDataDTO): Promise<void> {
  /* ... */
}
```

### Extract Class

```typescript
// Before: God object with many responsibilities
class UserManager {
  /* 500 lines */
}

// After: Focused classes
class UserValidator {
  /* validation logic */
}
class UserTransformer {
  /* transformation logic */
}
class UserRepository {
  /* persistence logic */
}
class UserService {
  /* orchestration */
}
```

### Replace Type Coercion with Type Safety

```typescript
// Before: Loose typing
function process(data: any): any {
  return data.map((item: any) => ({ ...item }));
}

// After: Strict typing
interface IProcessableItem {
  id: string;
  [key: string]: unknown;
}

function process(data: IProcessableItem[]): IProcessableItem[] {
  return data.map((item) => ({ ...item }));
}
```

### Apply Design Patterns

#### Observer Pattern

```typescript
// Before: Tight coupling
class UserService {
  save(user: User) {
    db.save(user);
    email.send(user.email);
    logger.log(user);
  }
}

// After: Loose coupling with observers
class UserService {
  private observers: IObserver[] = [];

  subscribe(observer: IObserver): void {
    this.observers.push(observer);
  }

  async save(user: User) {
    await db.save(user);
    this.observers.forEach((observer) => observer.onUserSaved(user));
  }
}
```

## Code Quality Metrics

- **Cyclomatic Complexity**: Aim < 10 per function
- **Lines of Code**: Aim < 100 per function
- **Function Parameters**: Aim ≤ 3 parameters
- **Test Coverage**: Aim ≥ 80% for business logic
- **Duplication**: Aim < 5% similar code blocks
