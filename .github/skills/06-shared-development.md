---
name: Shared Libraries & Package Development
description: Best practices for building reusable shared libraries, common utilities, and monorepo packages
keywords:
  - shared
  - libraries
  - monorepo
  - packages
  - utilities
  - types
  - modules
  - reuse
topics:
  - Shared Architecture
  - Package Versioning
  - Types and Interfaces
  - Utilities and Helpers
  - Dependency Management
  - Monorepo Patterns
applyTo:
  - /shared/**
relatedPrompts:
  - 01-feature-implementation.md
  - 03-refactoring.md
  - 05-testing-strategy.md
relatedSkills:
  - 01-web-development.md
  - 02-backend-development.md
  - 04-tools-development.md
version: 1.0
---

# Shared Libraries & Package Development

Guidance for creating reusable shared libraries and common packages across multiple projects.

## Shared Package Architecture

### Package Separation

- Keep shared code grouped by concern: `types`, `utils`, `constants`, `errors`, `logger`
- Each package should expose a clean public API
- Avoid circular dependencies between shared packages

### TypeScript Best Practices

- Use strict compiler options
- Export only type-safe interfaces
- Prefer `types/` for shared contract definitions
- Use `index.ts` for package exports

### Utility Libraries

- Keep helpers small and focused
- Document expected input/output
- Avoid global state in shared utilities
- Use tests to verify edge cases

## Monorepo Patterns

- Use workspaces for dependency management
- Keep package versions consistent when shared across projects
- Avoid duplicating shared packages in multiple locations
- Use symlinks or workspace references for local development

## Documentation for Shared Packages

- Provide package-level README files
- Describe exported modules and usage examples
- Show installation instructions for consumer projects
- Document compatibility and dependency requirements

## Testing Shared Packages

- Unit test reusable utilities and shared types
- Use type tests to validate exported interfaces
- Ensure package boundary behavior is stable
- Add regression tests for helper functions

## Deployment and Publishing

- Use semantic versioning for package releases
- Publish shared packages to private registries or npm if needed
- Keep build artifacts separate from source files
- Use workspace-aware build scripts when compiling multiple packages

## Key Principles

- Keep shared code stable and backward compatible
- Document usage clearly for downstream consumers
- Reduce duplication by centralizing common logic
- Use strong type contracts and consistent patterns

---

**Related Resources:**

- Prompts: [Feature Implementation](../prompts/01-feature-implementation.md), [Refactoring](../prompts/03-refactoring.md)
- Standards: [copilot-instructions.md](../copilot-instructions.md)
