# Project Setup Instructions

## Template Cleanup on First Project Description

When the user posts their first project idea or task description, immediately clean up this template without asking for confirmation.

### How to proceed

1. **Analyze** the user's project: what type (web, desktop, CLI, API, etc.), which workspaces are needed.
2. **Announce** what will be removed (one-line list, no question).
3. **Delete** all irrelevant folders and files immediately after announcing.
4. **Update** `package.json` workspaces array to match remaining packages.
5. **Update** `CLAUDE.md` — remove this "Template Cleanup" section after cleanup is done.

### What to always keep

- `.claude/` — Claude commands and settings
- `.github/workflows/` — CI/CD pipelines
- Root config: `.editorconfig`, `.gitignore`, `.gitattributes`, `.prettierrc.json`, `eslint.config.js`, `tsconfig.base.json`, `tsconfig.json`, `package.json`, `package-lock.json`
- `CLAUDE.md`, `LICENSE`

### What to remove if not needed by the project

| Folder/File                                            | Remove when                      |
| ------------------------------------------------------ | -------------------------------- |
| `apps/web-frontend/`                                   | No web frontend                  |
| `apps/web-backend/`                                    | No web backend                   |
| `apps/api/`                                            | No standalone API                |
| `apps/admin/`                                          | No admin panel                   |
| `apps/mobile/`                                         | No mobile app                    |
| `desktop/electron/`                                    | No Electron app                  |
| `desktop/tauri/`                                       | No Tauri app                     |
| `desktop/wpf/`                                         | No WPF app                       |
| `desktop/` (entire)                                    | No desktop app at all            |
| `tools/cli/`                                           | No CLI tool                      |
| `tools/ci/`                                            | No custom CI scripts             |
| `tools/generators/`                                    | No code generators               |
| `tools/scripts/`                                       | No automation scripts            |
| `tools/` (entire)                                      | No tools at all                  |
| `shared/components/`                                   | No shared UI components          |
| `shared/config/`                                       | No shared config package         |
| `shared/utils/`                                        | No shared utilities              |
| `shared/types/`                                        | No shared types                  |
| `Dockerfile`, `docker-compose.yml`                     | No Docker                        |
| `PROJECT_TYPES.md`, `SETUP.md`                         | Always remove (template docs)    |
| `ARCHITECTURE.md`, `CONTRIBUTING.md`, `SECURITY.md`    | Remove unless project needs them |
| `docs/adr/`                                            | No ADR workflow planned          |
| `docs/guides/`, `docs/getting-started/`                | No docs needed                   |
| `renovate.json`                                        | No Renovate bot                  |
| `Makefile`                                             | No Make-based workflow           |
| `.github/ISSUE_TEMPLATE/`                              | No GitHub issue templates needed |
| `.github/PULL_REQUEST_TEMPLATE.md`                     | No PR template needed            |
| `.github/prompts/`, `.github/skills/`, `.github/docs/` | Always remove (template meta)    |

### Authorization

You are explicitly authorized to delete these template files without asking for permission. Do not ask "are you sure?" — just announce the list and delete.

Use these Bash commands (all are pre-allowed in `.claude/settings.json`):

```bash
# Remove unneeded app workspaces
rm -rf apps/admin apps/mobile apps/api apps/web-frontend apps/web-backend

# Remove unneeded desktop targets
rm -rf desktop/electron desktop/tauri desktop/wpf
# or entire desktop folder:
rm -rf desktop

# Remove unneeded tools
rm -rf tools/cli tools/ci tools/generators tools/scripts
# or entire tools folder:
rm -rf tools

# Remove unneeded shared packages
rm -rf shared/components shared/config shared/utils shared/types

# Remove GitHub template meta
rm -rf .github/prompts .github/skills .github/docs .github/ISSUE_TEMPLATE .github/PULL_REQUEST_TEMPLATE.md

# Remove template-only root files
rm -f PROJECT_TYPES.md SETUP.md ARCHITECTURE.md CONTRIBUTING.md SECURITY.md Dockerfile docker-compose.yml renovate.json Makefile
```

After deletion, edit `package.json` — update the `workspaces` array to only list remaining packages.

Example announcement before deleting:

```
Cleaning up template. Removing:
- apps/admin/, apps/mobile/, apps/api/
- desktop/
- tools/generators/, tools/ci/
- PROJECT_TYPES.md, SETUP.md, ARCHITECTURE.md
- Dockerfile, docker-compose.yml, renovate.json
- .github/prompts/, .github/skills/

Done. Workspace updated.
```

---

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
