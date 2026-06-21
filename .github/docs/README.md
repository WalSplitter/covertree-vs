# GitHub Configuration & Development Guidelines

This directory contains all GitHub configuration, Copilot instructions, programming skills, and prompt templates for this project.

## Contents

### 📋 Main Documentation

- **[copilot-instructions.md](../copilot-instructions.md)** - Complete GitHub Copilot development guidelines
  - Code standards and conventions
  - Development workflow best practices
  - Prompt engineering techniques
  - Error handling strategies
  - Security and performance considerations

- **[SKILLS.md](../SKILLS.md)** - Programming patterns and techniques reference
  - Type safety and TypeScript best practices
  - Async programming patterns
  - Error handling implementations
  - API integration patterns
  - Testing strategies
  - Performance optimization techniques
  - Security implementation guide
  - Design patterns (Factory, Observer, Dependency Injection)

### 🎯 Prompt Templates

The `prompts/` directory contains specialized prompts for different development scenarios:

- **[00-quick-start.md](../prompts/00-quick-start.md)** - Getting started with Vibe Coding
  - Project type selection
  - Essential Copilot commands
  - Common development patterns
  - File organization reference
  - Troubleshooting tips

- **[01-feature-implementation.md](../prompts/01-feature-implementation.md)** - Feature development template
  - Context specification
  - Requirements definition
  - Constraint documentation
  - Deliverable checklist
  - Example usage walkthrough

- **[02-bug-fixing.md](../prompts/02-bug-fixing.md)** - Bug fixing and debugging template
  - Problem definition
  - Investigation approach
  - Solution strategies
  - Debugging checklist
  - Common fix patterns

- **[03-refactoring.md](../prompts/03-refactoring.md)** - Code quality and refactoring template
  - Current state assessment
  - Refactoring goals
  - Quality improvement metrics
  - Design pattern applications
  - Code quality metrics

- **[04-api-design.md](../prompts/04-api-design.md)** - API and integration design template
  - API specification structure
  - Request/response schemas
  - Implementation requirements
  - REST best practices
  - GraphQL integration guide

- **[05-testing-strategy.md](../prompts/05-testing-strategy.md)** - Testing approach template
  - Test planning
  - Mocking strategies
  - AAA pattern (Arrange-Act-Assert)
  - Integration testing
  - Coverage targets

- **[06-performance-optimization.md](../prompts/06-performance-optimization.md)** - Performance tuning template
  - Issue identification
  - Root cause analysis
  - Optimization strategies
  - Monitoring setup
  - Benchmarking approaches

## How to Use This Directory

### For Starting a New Feature

1. **Read Quick Start**: Start with [prompts/00-quick-start.md](../prompts/00-quick-start.md)
2. **Choose Template**: Select appropriate prompt from the `prompts/` directory
3. **Reference Guidelines**: Use [copilot-instructions.md](../copilot-instructions.md) for standards
4. **Check Skills**: Review [SKILLS.md](../SKILLS.md) for implementation patterns

### For Development Workflow

```
New Feature
    ↓
Read: 01-feature-implementation.md
Check: copilot-instructions.md (naming, structure)
Reference: SKILLS.md (patterns, examples)
Implement: Using Copilot with prompt
Test: Review 05-testing-strategy.md
    ↓
Bug Found
    ↓
Use: 02-bug-fixing.md
    ↓
Review Quality
    ↓
Use: 03-refactoring.md if needed
Use: 06-performance-optimization.md if needed
```

### For Code Reviews

Check against these standards:

- [ ] Naming follows conventions (camelCase, UPPER_SNAKE_CASE, PascalCase)
- [ ] Code follows SOLID principles
- [ ] Error handling is complete
- [ ] Type safety (strict TypeScript)
- [ ] Test coverage (80%+ for business logic)
- [ ] Performance implications reviewed
- [ ] Security best practices applied
- [ ] Documentation is clear (JSDoc for public APIs)

## Key Principles

### Professionalism

- Follow Microsoft code analysis style rules
- Implement SOLID principles consistently
- Maintain high code quality standards
- Document decisions and trade-offs

### Consistency

- Use naming conventions across the project
- Apply same patterns for similar problems
- Follow established code structure
- Keep dependencies organized

### Quality

- Type safety first (TypeScript strict mode)
- Test-driven development mindset
- Error handling by default
- Performance-conscious implementation

### Maintainability

- Small, focused functions
- Clear separation of concerns
- Comprehensive documentation
- Easy to understand and modify

## Technology Stack Guidance

### Frontend Development

- **Framework**: React, Vue, or Svelte
- **Language**: TypeScript
- **Styling**: CSS Modules, Tailwind CSS, or Styled Components
- **State Management**: Context API, Redux, or Zustand
- **Testing**: Jest, Vitest, or React Testing Library

### Backend Development

- **Runtime**: Node.js, .NET, or Python
- **Language**: TypeScript, C#, or Python
- **Framework**: Express, NestJS, ASP.NET Core, or FastAPI
- **Database**: PostgreSQL, MongoDB, or SQL Server
- **Testing**: Jest, pytest, or unittest

### Desktop Development

- **Framework**: Electron, Tauri, or WPF
- **Language**: TypeScript, C#, or Rust
- **Build Tool**: Webpack, Vite, or MSBuild

## Git Workflow

### Commit Message Format (Conventional Commits)

```
type(scope): subject

body

footer
```

**Types**: `feat`, `fix`, `docs`, `style`, `refactor`, `perf`, `test`, `chore`

**Example**:

```
feat(auth): implement two-factor authentication

Add TOTP-based 2FA support for user accounts.
Integrate with Google Authenticator and similar apps.

Closes #123
```

### Branch Naming

```
feature/user-authentication
bugfix/email-validation-regex
docs/api-documentation
refactor/payment-service
```

## External Resources

- [Microsoft Code Guidelines](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/)
- [Google TypeScript Style Guide](https://google.github.io/styleguide/tsguide.html)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [Clean Code Best Practices](https://clean-code-js.com/)
- [Testing Best Practices](https://testingjavascript.com/)

## Contributing to This Directory

When you discover new patterns, techniques, or improvements:

1. Document the pattern or technique
2. Add examples from the codebase
3. Create a new prompt template if applicable
4. Update this README with new resources
5. Share with the team

---

**Last Updated**: 2026  
**Version**: 1.0  
**Maintained by**: WalSplitter
