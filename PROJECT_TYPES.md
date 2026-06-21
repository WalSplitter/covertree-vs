# Project Type Guide

Quick reference for choosing the right project structure for your needs.

## Web Applications

### Use Case

Building full-stack web applications with separate frontend and backend.

### Technology

- **Frontend**: React, Vue, or Svelte with TypeScript
- **Backend**: Express, NestJS, or FastAPI
- **Database**: PostgreSQL, MongoDB
- **Deployment**: Vercel (frontend), Heroku/AWS (backend)

### Get Started

```bash
npm run dev -w @base-template/web-frontend
npm run dev -w @base-template/web-backend
```

### Best For

- Single-page applications (SPAs)
- Server-side rendered applications
- Real-time collaborative tools
- Content management systems
- Progressive web apps (PWAs)

---

## Backend Services & APIs

### Use Case

Building RESTful APIs, microservices, and backend-only projects.

### Technology

- **Framework**: Express, NestJS, or FastAPI
- **Database**: PostgreSQL, MongoDB, SQL Server
- **Cache**: Redis
- **Message Queue**: RabbitMQ, Apache Kafka
- **Deployment**: Docker, Kubernetes, AWS

### Get Started

```bash
cd apps/api-service/
npm install
npm run dev
```

### Best For

- Microservices architecture
- High-performance APIs
- Real-time services (WebSocket)
- Background job processing
- Mobile app backends

---

## Desktop Applications

### Use Case

Building cross-platform desktop applications.

### Technology

- **Electron** (TypeScript + React): Cross-platform, large app size
- **Tauri** (Rust + React/Vue): Lightweight, native performance
- **WPF** (C# + XAML): Windows-only, enterprise features
- **Swift** (SwiftUI): Native macOS applications

### Get Started

**Electron:**

```bash
cd desktop/electron-app/
npm install
npm run dev
```

**Tauri:**

```bash
cd desktop/tauri-app/
npm install
npm run tauri dev
```

### Best For

- Cross-platform applications
- Local file manipulation tools
- IDE/editor applications
- Development utilities
- Offline-first applications

---

## CLI Tools & Utilities

### Use Case

Building command-line tools, build scripts, and automation utilities.

### Technology

- **Commander.js** or **Yargs**: CLI frameworks
- **Chalk**: Terminal color output
- **Ora**: Loading spinners
- **Inquirer**: Interactive prompts
- **Sharp**: Image processing
- **csv-parser**: Data processing

### Get Started

```bash
cd tools/build-automation/
npm install
npm run cli -- help
```

### Best For

- Build automation
- Code generators
- Data processing scripts
- Database migrations
- Development workflows
- Package management

---

## Shared Libraries & Packages

### Use Case

Building reusable packages shared across multiple projects.

### Structure

- **Types**: Shared TypeScript types and interfaces
- **Utils**: Utility functions
- **Constants**: Application constants
- **Errors**: Custom error classes
- **Logger**: Logging utilities
- **Testing**: Test helpers and mocks

### Publishing

```bash
cd shared/types/
npm publish
# Install in other projects:
npm install @project/types
```

### Best For

- Monorepo architecture
- Shared domain models
- Common utilities
- Reusable error handling
- Shared logging infrastructure
- Type definitions

---

## Decision Matrix

| Need                         | Best Choice            | Directory                                |
| ---------------------------- | ---------------------- | ---------------------------------------- |
| Full-stack web app           | React + Express        | `apps/web-frontend` + `apps/web-backend` |
| Frontend only                | React/Vue/Svelte       | `apps/web-frontend`                      |
| Backend only                 | Express/NestJS/FastAPI | `/apps`                                  |
| REST API                     | NestJS/FastAPI         | `/apps/api-service`                      |
| Real-time API                | Socket.IO + Express    | `/apps/websocket-service`                |
| Background jobs              | Bull/Celery            | `/apps/worker-service`                   |
| Desktop app (cross-platform) | Electron/Tauri         | `/desktop`                               |
| Desktop app (Windows only)   | WPF                    | `/desktop`                               |
| CLI tool                     | Commander/Yargs        | `/tools`                                 |
| Code generator               | Templating engine      | `/tools/code-generator`                  |
| Shared types                 | TypeScript only        | `/shared/types`                          |
| Shared utilities             | Pure functions         | `/shared/utils`                          |
| Error handling               | Custom classes         | `/shared/errors`                         |

---

## Template Customization

### Remove Unused Directories

```bash
# Keep only what you need
rm -rf apps/web-frontend apps/web-backend  # If not building web app
rm -rf apps/         # If not building backend
rm -rf desktop/      # If not building desktop app
rm -rf tools/        # If not building CLI tools
```

### Update GitHub Branding

```bash
# Edit .github/README.md with your organization name
# Update LICENSE if needed
# Modify README.md project description
```

### Configure Monorepo (Optional)

```json
{
  "workspaces": ["apps/*", "desktop/*", "tools/*", "shared/*"]
}
```

---

## Development Workflow by Project Type

### Web Application

1. Read: `apps/web-frontend/` and `apps/web-backend/`
2. Choose frontend framework
3. Choose backend framework
4. Setup database
5. Use `.github/prompts/01-feature-implementation.md`

### Backend Service

1. Read: `/apps/README.md`
2. Choose framework (Express/NestJS/FastAPI)
3. Setup database
4. Use `.github/prompts/04-api-design.md`

### Desktop Application

1. Read: `/desktop/README.md`
2. Choose framework (Electron/Tauri/WPF)
3. Setup build process
4. Test on multiple platforms

### CLI Tool

1. Read: `/tools/README.md`
2. Choose CLI framework
3. Define commands and options
4. Build command handlers
5. Setup publishing (npm/PyPI)

### Shared Library

1. Read: `/shared/README.md`
2. Define public API
3. Add JSDoc documentation
4. Setup publish workflow
5. Version according to semver

---

## Getting Help

- **Feature Implementation**: See `.github/prompts/01-feature-implementation.md`
- **Bug Fixing**: See `.github/prompts/02-bug-fixing.md`
- **Testing**: See `.github/prompts/05-testing-strategy.md`
- **Performance**: See `.github/prompts/06-performance-optimization.md`
- **Architecture**: See `./docs/architecture.md`
- **Troubleshooting**: See `./docs/troubleshooting.md`

---

**Last Updated**: 2026  
**Version**: 1.0
