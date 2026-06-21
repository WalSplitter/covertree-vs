# ADR-001: Monorepo Structure with npm Workspaces

**Status**: Accepted

**Date**: 2026-06-01

**Deciders**: Architecture Team

---

## Context

The project needs to support multiple applications (web, desktop, CLI tools) while maintaining:

- Code reusability across projects (shared types, utilities, components)
- Independent build and deployment pipelines
- Clear separation of concerns
- Easy dependency management

Multiple solutions exist: monorepo, polyrepo, or standalone packages.

## Decision

**We will use npm Workspaces for monorepo management** rather than Lerna or separate repositories.

This enables:

1. Unified Node.js tooling experience
2. Shared dependencies at root level
3. Efficient testing and linting across packages
4. Clear workspace definitions in package.json
5. Native npm support (no external tool required)

### Implementation

Root `package.json` defines workspaces:

```json
{
  "workspaces": ["apps/*", "desktop/*", "tools/*", "shared/*"]
}
```

Each workspace has its own `package.json` for specific dependencies.

## Consequences

### Positive

- ✅ Simplified tooling (native npm support)
- ✅ Easier onboarding for new developers
- ✅ Better code reusability
- ✅ Unified deployment process
- ✅ Shared TypeScript and ESLint configuration
- ✅ Reduced node_modules footprint

### Negative

- ⚠️ Requires consistent Node.js version across workspaces
- ⚠️ Shared dependencies can cause version conflicts
- ⚠️ More complex CI/CD configuration

### Risks

- **Dependency conflicts**: Mitigated by root-level lock file
- **Performance**: Mitigated by proper caching in CI/CD
- **Complexity**: Mitigated by clear documentation and examples

## Alternatives Considered

### Alternative 1: Polyrepo (Separate Repositories)

Pros:

- Independent versioning and releases
- Cleaner separation of concerns

Cons:

- Difficult to share code between projects
- Complex dependency management
- Duplicated configuration

Why rejected: Increases maintenance burden and reduces code reusability.

### Alternative 2: Lerna + npm Workspaces

Pros:

- Powerful versioning and publishing tools
- Community standard for monorepos

Cons:

- Additional learning curve
- Extra tooling complexity
- Slower for small teams

Why rejected: Overkill for current needs; native npm workspaces are sufficient.

### Alternative 3: Yarn/pnpm Workspaces

Pros:

- Better performance
- Stricter dependency management

Cons:

- Requires learning different package manager
- Less common in enterprise environments
- Integration issues with tooling

Why rejected: npm is standard; no compelling reason to switch at this stage.

## Related Decisions

- ADR-002: TypeScript Strict Mode
- ADR-003: API Design
- [Monorepo Setup Guide](../guides/monorepo-setup.md)

## References

- [npm Workspaces Documentation](https://docs.npmjs.com/cli/v8/using-npm/workspaces)
- [Monorepo Benefits](https://monorepo.tools/)
- [Google Monorepo Architecture](https://blog.google/products/chrome/how-we-organize-code-google-scale/)

---

**Last Updated**: 2026-06-01
