# Quick Start – Base Project Template

Quick reference for efficient development with Claude in this monorepo.

## Project Setup

```bash
npm install
cp .env.example .env
npm run dev --workspaces
# Frontend: http://localhost:5173
# Backend:  http://localhost:3000
```

## Choose Your Workspace

```
Web Application (React + Express)       → apps/web-frontend + apps/web-backend
Backend Services (Node.js + TypeScript) → /apps/*
Desktop Application (Electron/Tauri)    → /desktop/*
Command-Line Tool (Node.js CLI)         → /tools/*
Shared Libraries (Types/Utils)          → /shared/*
```

## Development Patterns

### Feature Development (~30 min)

1. Define types in `shared/types/src/` first
2. Implement core logic with error handling
3. Write unit tests (Vitest, 80%+ coverage)
4. Document with JSDoc (`@param`, `@returns`, `@throws`)
5. Commit: `feat(scope): description`

### Bug Investigation (~20 min)

1. Reproduce with clear steps
2. Run `npm run type-check --workspaces`
3. Locate source in correct workspace
4. Implement fix + regression test
5. Commit: `fix(scope): description`

### Code Review (~10 min)

- Naming conventions followed?
- Error handling complete?
- Types strict (no `any`)?
- Tests cover happy path + edge cases?

## Common Commands

```bash
npm run dev -w @base-template/web-frontend   # Start frontend
npm run dev -w @base-template/web-backend    # Start backend
npm run test:watch -w @base-template/web-backend  # Tests in watch mode
npm run test:coverage -w @base-template/web-backend  # Coverage report
npm run type-check --workspaces      # Check all types
npm run lint:fix -w @base-template/web-frontend  # Fix frontend linting
npm run lint:fix -w @base-template/web-backend   # Fix backend linting
make docker-up                       # Start Postgres + Redis
```

## Available Slash Commands

- `/project:feature` – Implement a new feature
- `/project:bug` – Debug and fix an issue
- `/project:refactor` – Improve code quality
- `/project:api` – Design an API endpoint
- `/project:tests` – Write test cases
- `/project:perf` – Optimize performance
- `/project:web` – Web development patterns
- `/project:backend` – Backend patterns
- `/project:desktop` – Desktop app patterns
- `/project:tools` – CLI tool patterns
- `/project:shared` – Shared library patterns
