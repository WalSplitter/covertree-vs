# Contributing Guide

Thank you for your interest in contributing to this template! This guide explains how to maintain code quality and professional standards.

## Code of Conduct

- Be respectful and inclusive
- Welcome different perspectives
- Report issues constructively
- Help others learn and grow

## Getting Started

1. Fork the repository
2. Clone your fork: `git clone https://github.com/WalSplitter/base-project-template`
3. Create a feature branch: `git checkout -b feature/your-feature`
4. Read the development guidelines: `.github/copilot-instructions.md`

## Development Standards

### Code Quality

- ✅ TypeScript with strict mode enabled
- ✅ ESLint configuration applied
- ✅ Prettier formatting consistent
- ✅ 80%+ test coverage for new code
- ✅ No console.log in production code
- ✅ Error handling implemented

### Naming Conventions

```typescript
// Variables & Functions: camelCase
const userName = 'John Doe';
function calculateTotal() {}

// Classes & Types: PascalCase
class UserService {}
interface IUserProfile {}

// Constants: UPPER_SNAKE_CASE
const MAX_RETRIES = 3;

// Files & Folders: kebab-case
src / user - service.ts;
src / components / user - profile.tsx;
```

### File Organization

```
src/
├── domain/          # Business logic
├── infrastructure/  # External integrations
├── api/            # API layer
├── types/          # Type definitions
├── utils/          # Utilities
└── index.ts        # Public exports
```

## Commit Guidelines

### Conventional Commits Format

```
type(scope): subject

body

footer
```

### Types

- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (no logic change)
- `refactor`: Code restructuring (no feature change)
- `perf`: Performance improvements
- `test`: Test additions/modifications
- `chore`: Build, dependency, or tooling changes

### Examples

```bash
# Feature
git commit -m "feat(auth): add two-factor authentication"

# Bug fix
git commit -m "fix(api): handle null user email correctly"

# Documentation
git commit -m "docs(readme): update installation instructions"

# Refactoring
git commit -m "refactor(user-service): extract validation logic"

# Performance
git commit -m "perf(database): add query indexing for user lookups"
```

## Pull Request Process

### Before Creating PR

1. Ensure all tests pass: `npm run test`
2. Check code quality: `npm run lint`
3. Format code: `npm run format`
4. Build successfully: `npm run build`
5. Update documentation if needed

### PR Description Template

```markdown
## Description

Brief description of changes

## Type of Change

- [ ] New feature
- [ ] Bug fix
- [ ] Breaking change
- [ ] Documentation update

## Related Issue

Closes #123

## Changes Made

- Change 1
- Change 2

## Testing

Describe testing performed

## Checklist

- [ ] Code follows style guidelines
- [ ] Tests added/updated
- [ ] Documentation updated
- [ ] No breaking changes (or documented)
```

### Review Process

1. **Automated checks**: Linting, tests, build must pass
2. **Code review**: Team members review for quality
3. **Approval**: Requires at least 1 approval
4. **Merge**: Squash and merge strategy

## Testing Requirements

### Unit Tests

```typescript
describe('UserService', () => {
  it('should create user with valid email', async () => {
    const result = await userService.create({
      email: 'test@example.com',
      name: 'Test User',
    });
    expect(result.id).toBeDefined();
  });

  it('should throw ValidationError for invalid email', async () => {
    await expect(
      userService.create({
        email: 'invalid',
        name: 'Test',
      })
    ).rejects.toThrow(ValidationError);
  });
});
```

### Coverage Goals

- **Business Logic**: 80%+ coverage
- **Utilities**: 85%+ coverage
- **API Handlers**: 70%+ coverage
- **Infrastructure**: 50%+ coverage
- **Generated Code**: Excluded

## Documentation Requirements

### Code Comments

```typescript
// ✅ Good: Explains why
// Use exponential backoff to prevent overwhelming the server
const delay = Math.pow(2, attempt) * 1000;

// ❌ Bad: States the obvious
// Add 1000 to delay
delay += 1000;
```

### JSDoc for Public APIs

```typescript
/**
 * Creates a new user account.
 *
 * @param email - User email address (must be unique)
 * @param password - User password (min 8 characters)
 * @returns Created user object with ID
 * @throws {ValidationError} If email/password invalid
 * @throws {ConflictError} If email already exists
 *
 * @example
 * const user = await createUser('user@example.com', 'password123');
 */
export async function createUser(email: string, password: string): Promise<IUser> {
  // Implementation
}
```

### README Updates

Update relevant README files when:

- Adding new directory/feature
- Changing how to run/build/test
- Updating dependencies
- Breaking changes introduced

## Reporting Issues

### Bug Report

```markdown
**Description**: Clear description of the bug

**Steps to Reproduce**:

1. Do X
2. Do Y
3. See Z

**Expected**: What should happen
**Actual**: What actually happens
**Environment**: OS, Node version, etc.

**Screenshots**: If applicable
```

### Feature Request

```markdown
**Description**: What feature would help?
**Use Case**: Why is this needed?
**Proposed Solution**: How should it work?
**Alternatives**: Other approaches considered
```

## Improving This Template

This is a living template. Contributions to improve it are welcome:

1. **Bug Fixes**: Report issues you find
2. **Documentation**: Clarify unclear sections
3. **New Patterns**: Add useful programming patterns
4. **Prompt Templates**: Suggest new Copilot prompt templates
5. **Examples**: Add real-world usage examples

## Getting Help

- **Questions**: Use GitHub Discussions
- **Issues**: Report bugs in Issues
- **Security**: Email security@example.com (don't create public issue)
- **General**: See README.md for documentation

## License

By contributing, you agree that your contributions will be licensed under the same license as the project (MIT).

---

**Last Updated**: 2026  
**Version**: 1.0

Thank you for helping improve this template! 🙏
