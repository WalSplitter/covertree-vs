# GitHub Copilot Development Guidelines

Professional development standards and best practices for this project using GitHub Copilot and Microsoft guidelines.

## Code Standards

### General Principles

- **Clean Code**: Write readable, maintainable code following SOLID principles
- **DRY (Don't Repeat Yourself)**: Eliminate code duplication through abstractions
- **KISS (Keep It Simple, Stupid)**: Prioritize simplicity over complexity
- **Explicit Over Implicit**: Make intent clear in code
- **Fail Fast**: Validate inputs and fail early with meaningful errors

### Naming Conventions

#### Variables & Constants

- Use **camelCase** for variables and functions
- Use **UPPER_SNAKE_CASE** for constants
- Use **PascalCase** for classes, types, and interfaces
- Avoid single-letter names except for loop counters
- Use descriptive names that convey purpose

```typescript
// Good
const MAX_RETRY_ATTEMPTS = 3;
let userProfileData: IUserProfile;
function calculateTotalPrice() {}

// Avoid
const mrad = 3;
let upd: any;
function calc() {}
```

#### File & Folder Organization

- Use **kebab-case** for file and folder names
- Group related functionality together
- Keep modules focused and single-purpose
- Organize by feature, not by type

### Code Organization

#### File Structure

```
module/
├── index.ts          # Public exports
├── types.ts          # Type definitions
├── constants.ts      # Module constants
├── module.ts         # Core implementation
├── module.test.ts    # Tests
├── utils.ts          # Helper functions (if needed)
└── README.md         # Module documentation
```

#### Imports Organization

```typescript
// 1. Standard library imports
import fs from 'fs';
import path from 'path';

// 2. Third-party imports
import axios from 'axios';
import { validation } from 'class-validator';

// 3. Local imports
import { UserService } from './services';
import { IConfig } from './types';
import { logger } from '@shared/logger';

// 4. Relative imports
import { helper } from '../utils';
```

## Development Workflow

### Before Writing Code

1. **Understand Requirements**: Clearly define what needs to be built
2. **Plan Architecture**: Sketch the solution structure
3. **Check Existing Patterns**: Review similar implementations in the codebase
4. **Define Types/Interfaces**: Establish contracts first

### During Development

1. **Write Tests First**: Consider test cases before implementation (TDD mindset)
2. **Use Type Safety**: Leverage TypeScript/static typing
3. **Document Complex Logic**: Add comments explaining "why", not "what"
4. **Keep Functions Small**: Each function should do one thing well
5. **Error Handling**: Handle all error paths explicitly

### Code Review Checklist

- [ ] Code follows naming conventions
- [ ] Functions have clear purpose and documentation
- [ ] Error handling is complete
- [ ] No code duplication
- [ ] Tests pass and cover happy/error paths
- [ ] Performance implications considered
- [ ] Security vulnerabilities checked
- [ ] Types are properly defined (TypeScript)

## GitHub Copilot Resources

Use the repository’s prompt templates and skill guides together to keep Copilot suggestions aligned with project conventions.

- `.github/prompts/` contains structured prompt templates for feature implementation, bug fixing, refactoring, API design, testing, and performance.
- `.github/skills/` contains domain-specific Copilot skill guides for web, backend, desktop, tools, docs, and shared package development.
- Refer to `.github/skills/README.md` for an index of available skill guides.

### Recommended workflow

1. Start with the prompt structure from `.github/prompts/`.
2. Reference the relevant skill guide in `.github/skills/` for domain-specific patterns.
3. Use `copilot-instructions.md` for general standards, naming conventions, and review checklists.

## Prompt Engineering Best Practices

### Effective Prompts for Copilot

#### 1. Be Specific and Clear

```
✓ "Create a TypeScript utility function that validates email addresses using regex
   and returns both validation result and error message"
✗ "Make an email validator"
```

#### 2. Provide Context

- Show existing code patterns to maintain consistency
- Include relevant type definitions
- Reference related implementations

#### 3. Include Examples

```
✓ "Create a function similar to calculateDiscount() that converts currencies
   Input: { amount: number, fromCurrency: string, toCurrency: string }
   Output: { convertedAmount: number, rate: number }"
```

#### 4. Specify Constraints

- Technology stack (TypeScript, React, Vue, etc.)
- Error handling requirements
- Performance considerations
- Specific libraries/frameworks to use or avoid

#### 5. Request Iterative Refinement

```
"Generate a basic version first, then optimize for performance"
"Start with a simple implementation, add error handling next"
```

### Prompt Structure Template

```
[Context] - What are we building and why?
[Requirements] - What specific functionality is needed?
[Constraints] - Technology, patterns, or style guidelines
[Examples] - Show desired input/output or similar code
[Deliverables] - What code/documentation is expected?
```

## Documentation

### Code Comments

```typescript
// Good: Explains why, not what
// Retry logic: exponential backoff prevents server overload during recovery
const delay = Math.pow(2, attemptNumber) * 1000;

// Avoid: Restates the code
// Add 100 to x
const result = x + 100;
```

### Function Documentation

Use JSDoc format for public APIs:

```typescript
/**
 * Fetches user data and applies caching strategy.
 *
 * @param userId - The unique user identifier
 * @param useCache - Whether to use cached data (default: true)
 * @returns Promise resolving to user profile data
 * @throws {UserNotFoundError} When user doesn't exist
 * @throws {NetworkError} When API request fails
 *
 * @example
 * const user = await getUserProfile('user123');
 * const freshUser = await getUserProfile('user123', false);
 */
function getUserProfile(userId: string, useCache = true): Promise<IUserProfile> {
  // Implementation
}
```

### Module README

Each significant module should have a README.md explaining:

- **Purpose**: What does this module do?
- **Usage**: How to use it?
- **API**: Public functions/classes
- **Examples**: Code samples
- **Dependencies**: External requirements

## Testing Standards

### Test Organization

```
src/
├── module/
│   ├── module.ts
│   ├── module.test.ts      # Unit tests
│   └── module.integration.test.ts
└── __tests__/
    └── integration/        # Cross-module tests
```

### Test Naming

```typescript
describe('UserService', () => {
  describe('getUserProfile', () => {
    it('should return user profile when user exists', () => {});
    it('should throw UserNotFoundError when user does not exist', () => {});
    it('should cache result on second call', () => {});
  });
});
```

### Test Coverage Goals

- **Unit Tests**: 80%+ coverage of business logic
- **Integration Tests**: Critical user workflows
- **E2E Tests**: Main application flows

## Error Handling

### Strategy

```typescript
// 1. Use custom error classes
class ValidationError extends Error {
  constructor(
    public field: string,
    message: string
  ) {
    super(message);
  }
}

// 2. Handle at appropriate layer
try {
  const user = await getUserProfile(userId);
} catch (error) {
  if (error instanceof UserNotFoundError) {
    // Handle not found
  } else if (error instanceof NetworkError) {
    // Handle network issue
  } else {
    // Log unexpected error
    logger.error('Unexpected error:', error);
  }
}

// 3. Provide meaningful messages
throw new ValidationError('email', 'Invalid email format. Expected: user@domain.com');
```

## Performance Considerations

- **Memoization**: Cache expensive computations
- **Lazy Loading**: Load resources when needed
- **Debouncing/Throttling**: Control event handler frequency
- **Pagination**: Handle large datasets efficiently
- **Async Operations**: Use concurrency appropriately
- **Monitoring**: Track performance metrics

## Security Best Practices

- **Input Validation**: Always validate external input
- **Type Safety**: Use TypeScript strict mode
- **Secrets Management**: Use environment variables for secrets
- **SQL Injection Prevention**: Use parameterized queries
- **XSS Protection**: Sanitize user input in UI
- **Authentication**: Implement proper auth mechanisms
- **Authorization**: Verify permissions before actions
- **Logging**: Log security events without exposing secrets

## Technology Stack Guidance

### Frontend

- **Framework**: React, Vue, or Svelte
- **Language**: TypeScript
- **Styling**: CSS Modules, Tailwind, or Styled Components
- **State Management**: As needed (Context, Redux, Zustand)
- **Testing**: Jest, Vitest, React Testing Library

### Backend

- **Runtime**: Node.js, .NET, or Python
- **Language**: TypeScript, C#, or Python
- **Framework**: Express, NestJS, ASP.NET Core, or FastAPI
- **Database**: PostgreSQL, MongoDB, or SQL Server
- **Testing**: Jest, unittest, pytest

### Desktop

- **Framework**: Electron, Tauri, or WPF
- **Language**: TypeScript, C#, or Rust
- **Build Tool**: Webpack, Vite, or MSBuild

## Git Workflow

### Commit Messages

Follow Conventional Commits format:

```
type(scope): subject

body

footer
```

Types: `feat`, `fix`, `docs`, `style`, `refactor`, `perf`, `test`, `chore`

```
feat(auth): add two-factor authentication

Implement TOTP-based 2FA for user accounts.
Supports Google Authenticator and similar apps.

Closes #123
```

### Branch Naming

```
feature/user-authentication
bugfix/email-validation-regex
docs/api-documentation
```

## References & Resources

- [Microsoft Code Guidelines](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/)
- [Google TypeScript Style Guide](https://google.github.io/styleguide/tsguide.html)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [Clean Code Best Practices](https://clean-code-js.com/)

---

**Last Updated**: 2026
**Version**: 1.0
