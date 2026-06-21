# Base Project Template

[![GitHub](https://img.shields.io/badge/GitHub-base--project--template-blue?logo=github)](https://github.com/WalSplitter/base-project-template)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.0+-blue.svg)](https://www.typescriptlang.org/)
[![Node.js](https://img.shields.io/badge/Node.js-18+-green.svg)](https://nodejs.org/)
[![npm workspaces](https://img.shields.io/badge/npm-workspaces-blue.svg)](https://docs.npmjs.com/cli/v8/using-npm/workspaces)
[![GitHub Actions](https://img.shields.io/badge/CI/CD-GitHub%20Actions-blue.svg)](https://github.com/features/actions)

> A production-ready, enterprise-grade template for building scalable applications with modern best practices, GitHub Copilot optimization, and professional development standards.

## 🎯 Overview

This comprehensive template repository provides everything you need to build professional applications at scale:

- **🏗️ Monorepo Architecture**: Multiple project types in a single repository with shared dependencies
- **📋 TypeScript-First**: Strict type safety, full IntelliSense, and excellent developer experience
- **🤖 GitHub Copilot Integration**: Optimized prompts, skill guides, and development patterns
- **🔄 CI/CD Ready**: Pre-configured GitHub Actions for testing, linting, security scanning
- **🐳 Docker Support**: Multi-stage Dockerfile and docker-compose for local development
- **📚 Complete Documentation**: Setup guides, deployment strategies, ADRs, and troubleshooting

### Perfect For

- 🌐 **Web Applications** - Full-stack React/Vue/Svelte with Express/NestJS backends
- 🖥️ **Backend Services** - Microservices, APIs, and scalable server applications
- 💻 **Desktop Applications** - Cross-platform apps with Electron or Tauri
- 🔧 **CLI Tools** - Command-line utilities and code generators
- 📦 **Shared Libraries** - Reusable types, utilities, and components
- 🚀 **Monorepo Projects** - Multiple applications with shared code
- 📱 **Progressive Web Apps** - PWA and hybrid applications

## 🔗 Quick Links

**Start Here:**

- 🚀 [Quick Start (5 min)](./docs/getting-started/README.md)
- ⚙️ [Environment Setup](./docs/getting-started/environment.md)
- 📖 [Development Guidelines](./.github/docs/README.md)

**Development:**

- 🤖 [GitHub Copilot Prompts](./.github/docs/README.md)
- 🤖 [Claude Commands](./.claude/commands/)
- 🏗️ [Architecture & ADRs](./docs/adr/)
- 📋 [API Design Guide](./.github/prompts/04-api-design.md)

**Deployment & DevOps:**

- 🐳 [Docker Setup](./docs/guides/deployment.md#local-development-with-docker)
- 🚢 [Deploy to Production](./docs/guides/deployment.md)
- 🔐 [Security Best Practices](./SECURITY.md)

## ✨ Key Features

### 🎯 Out-of-the-Box Setup

- ✅ **npm Workspaces** - Efficient monorepo management with unified dependencies
- ✅ **TypeScript 5.0+** - Strict mode enabled with full ESLint configuration
- ✅ **Code Quality** - ESLint (flat config), Prettier, and automated formatting pre-configured
- ✅ **Git Ready** - .gitattributes, .gitignore, and conventional commits setup
- ✅ **Environment Templates** - .env.example with sensible defaults

### 🤖 GitHub Copilot & Claude Optimization

- 📝 **Development Guidelines** - Complete coding standards and best practices
- 🎓 **Skill Guides** - Domain-specific patterns (web, backend, desktop, tools, docs, shared)
- 📋 **Prompt Templates** - Pre-written prompts for feature implementation, bug fixes, refactoring, testing
- 🏗️ **Architecture Patterns** - SOLID principles, dependency injection, error handling
- 📚 **Reference Documentation** - Type safety, async patterns, API design, performance optimization
- 🧠 **Claude Commands** - Slash-style commands in `.claude/commands/` for Claude-powered workflows

### 🚀 CI/CD & DevOps

- 🔄 **GitHub Actions Workflows** - Automated testing, linting, building, and security scanning
- 🐳 **Docker Support** - Multi-stage Dockerfile for production builds + docker-compose for local dev
- 🎯 **Makefile** - Common development commands for quick access
- 🔐 **Security Integration** - Dependabot config, vulnerability scanning, secret management
- 📊 **Code Quality** - Automated code analysis and coverage reporting

### 📦 Project Templates

- **Web Frontend** - React + Vite + TypeScript with routing and state management
- **Web Backend** - Express + TypeScript with middleware, routing, and error handling
- **API Service** - Standalone RESTful API service template
- **Shared Types** - Centralized TypeScript definitions for type-safe communication
- **Desktop Ready** - Structure for Electron, Tauri, or WPF applications
- **CLI Tools** - Scaffolding for command-line utilities

### 📚 Complete Documentation

- 🚀 [Quick Start Guide](./docs/getting-started/README.md) - Get running in 5 minutes
- 🔧 [Environment Setup](./docs/getting-started/environment.md) - Development environment configuration
- 🏗️ [Architecture Decisions](./docs/adr/) - ADRs for design choices
- 📖 [Deployment Guides](./docs/guides/deployment.md) - Vercel, AWS, Railway, Docker
- 🔐 [Security Policy](./SECURITY.md) - Best practices and vulnerability reporting
- 📋 [Setup Instructions](./SETUP.md) - Detailed installation and configuration

## ✅ Prerequisites

**Required:**

- **Node.js 18+** - [Download](https://nodejs.org/)
- **npm 9+** - Comes with Node.js
- **Git** - [Download](https://git-scm.com/)

**Optional (for full setup):**

- **Docker 24+** - [Download](https://www.docker.com/products/docker-desktop) (for PostgreSQL + Redis)
- **Visual Studio Code** - [Download](https://code.visualstudio.com/) (recommended)

**Verify Installation:**

```bash
node --version   # Should be 18.0+
npm --version    # Should be 9.0+
git --version    # Any recent version
```

## 🚀 Quick Start

### 1️⃣ Create Your Project from Template

**Option A: GitHub Template (Recommended)**

```bash
# Use "Use this template" button on GitHub:
# https://github.com/WalSplitter/base-project-template

# Click: "Use this template" → "Create a new repository"
# Then clone your new repository:
git clone https://github.com/YOUR_USERNAME/my-project.git
cd my-project
```

**Option B: GitHub CLI**

```bash
# Create new repo from template
gh repo create my-project --template WalSplitter/base-project-template --public
cd my-project
```

**Option C: Direct Clone & Update Remote**

```bash
# Clone this template
git clone https://github.com/WalSplitter/base-project-template.git my-project
cd my-project

# Update git remote to your repository
git remote set-url origin https://github.com/YOUR_USERNAME/my-project.git
git push -u origin main
```

### 2️⃣ Install & Setup (5 minutes)

```bash
# Install all dependencies (includes all workspaces)
npm install

# Copy environment templates
cp .env.example .env
cp apps/web-backend/.env.example apps/web-backend/.env
cp apps/web-frontend/.env.example apps/web-frontend/.env

# Start development servers (frontend on :5173, backend on :3000)
npm run dev
```

**Verify Setup:**

- Frontend: http://localhost:5173
- Backend API: http://localhost:3000/api/health
- Check status: `npm run type-check`

### 3️⃣ Environment Configuration

**Essential .env Variables:**

```bash
# Backend (apps/web-backend/.env)
NODE_ENV=development
PORT=3000
LOG_LEVEL=debug

# Database (needs Docker: make docker-up)
DATABASE_URL=postgresql://developer:password@localhost:5432/dev_db
REDIS_URL=redis://localhost:6379

# Authentication
JWT_SECRET=your-secret-key-change-in-production
JWT_EXPIRES_IN=7d

# Frontend (apps/web-frontend/.env)
VITE_API_URL=http://localhost:3000/api
VITE_ENV=development
```

**See [.env.example](./.env.example) and workspace .env.example files for all available options.**

### 4️⃣ Choose Your Path

| Goal                 | Workspace                                | Command                                      | Port                             |
| -------------------- | ---------------------------------------- | -------------------------------------------- | -------------------------------- |
| **Full-Stack Web**   | `apps/web-frontend` + `apps/web-backend` | `npm run dev --workspaces`                   | 5173 (frontend) + 3000 (backend) |
| **Backend API Only** | `apps/web-backend`                       | `npm run dev -w @base-template/web-backend`  | 3000                             |
| **Frontend Only**    | `apps/web-frontend`                      | `npm run dev -w @base-template/web-frontend` | 5173                             |
| **API Service**      | `apps/api`                               | `npm run dev -w @base-template/api`          | configurable                     |
| **Desktop App**      | `desktop/`                               | See [desktop guide](./desktop/README.md)     | Varies                           |
| **CLI Tool**         | `tools/`                                 | See [tools guide](./tools/README.md)         | N/A                              |

### 5️⃣ Workspace Commands Reference

**Run commands in specific workspaces:**

```bash
# Development
npm run dev -w @base-template/web-frontend
npm run dev -w @base-template/web-backend
npm run dev -w @base-template/api
npm run dev --workspaces

# Testing
npm run test -w @base-template/web-frontend
npm run test -w @base-template/web-backend
npm run test -w @base-template/api
npm run test:watch --workspaces
npm run test:coverage --workspaces
npm run test --workspaces

# Building
npm run build -w @base-template/web-frontend
npm run build -w @base-template/web-backend
npm run build -w @base-template/api
npm run build --workspaces

# Code Quality
npm run lint --workspaces
npm run lint -w @base-template/web-frontend
npm run lint -w @base-template/web-backend
npm run lint -w @base-template/api
```

**Common global commands:**

```bash
npm install              # Install all dependencies
npm run dev              # Start all workspaces
npm run build            # Build all workspaces
npm run test             # Test all workspaces
npm run lint             # Lint all workspaces
npm run type-check       # Type check all
npm run format:check     # Format check all
make docker-up           # Start PostgreSQL & Redis
make docker-down         # Stop containers
make help                # Show all Makefile commands
```

### 6️⃣ Essential Reading

1. 📖 **[Development Guidelines](./.github/docs/README.md)** - Code standards & practices
2. 🤖 **[Quick Start Prompts](./.github/prompts/00-quick-start.md)** - Using GitHub Copilot effectively
3. 🧠 **[Claude Commands](./.claude/commands/)** - Using Claude for development tasks
4. 🏗️ **[Architecture Guide](./ARCHITECTURE.md)** - Design decisions & patterns
5. 📚 **[Setup Instructions](./SETUP.md)** - Detailed configuration

### 7️⃣ First Contribution

1. Create feature branch: `git checkout -b feature/my-feature`
2. Make changes following [guidelines](./.github/docs/README.md)
3. Test locally: `npm run test && npm run lint`
4. Commit using conventional format: `git commit -m "feat(module): add feature"`
5. Push and create PR: `git push origin feature/my-feature`

## 📁 Project Structure

```
base-project-template/
├── .claude/
│   ├── commands/                  # Claude slash-style commands
│   │   ├── api.md
│   │   ├── backend.md
│   │   ├── bug.md
│   │   ├── desktop.md
│   │   ├── feature.md
│   │   ├── perf.md
│   │   ├── quick-start.md
│   │   ├── refactor.md
│   │   ├── shared.md
│   │   ├── tests.md
│   │   ├── tools.md
│   │   └── web.md
│   ├── docs/
│   │   └── README.md              # Claude-specific documentation
│   └── settings.json              # Claude settings
│
├── .github/
│   ├── workflows/                 # CI/CD pipelines
│   │   ├── ci.yml                 # Build, test, lint, security
│   │   └── code-quality.yml       # Code quality analysis
│   ├── ISSUE_TEMPLATE/            # Issue templates
│   │   ├── bug.yml
│   │   └── feature.yml
│   ├── PULL_REQUEST_TEMPLATE.md   # PR template
│   ├── docs/
│   │   └── README.md              # Complete development guidelines & prompts
│   ├── copilot-instructions.md    # Development standards & guidelines
│   ├── SKILLS.md                  # Programming patterns & techniques
│   ├── prompts/                   # GitHub Copilot prompts
│   │   ├── 00-quick-start.md
│   │   ├── 01-feature-implementation.md
│   │   ├── 02-bug-fixing.md
│   │   ├── 03-refactoring.md
│   │   ├── 04-api-design.md
│   │   ├── 05-testing-strategy.md
│   │   └── 06-performance-optimization.md
│   └── skills/                    # Domain-specific guides
│       ├── 01-web-development.md
│       ├── 02-backend-development.md
│       ├── 03-desktop-development.md
│       ├── 04-tools-development.md
│       ├── 05-documentation-development.md
│       ├── 06-shared-development.md
│       └── README.md
│
├── apps/                          # Applications
│   ├── web-frontend/              # React + Vite frontend
│   │   ├── src/
│   │   │   ├── components/
│   │   │   ├── styles/
│   │   │   └── App.tsx
│   │   ├── package.json
│   │   ├── vite.config.ts
│   │   └── tsconfig.json
│   ├── web-backend/               # Express backend
│   │   ├── src/
│   │   │   ├── middleware/
│   │   │   ├── routes/
│   │   │   └── utils/
│   │   ├── package.json
│   │   ├── vitest.config.ts
│   │   └── tsconfig.json
│   ├── api/                       # Standalone API service template
│   ├── mobile/                    # Mobile backend
│   ├── admin/                     # Admin service
│   └── README.md
│
├── desktop/                       # Desktop applications
│   ├── electron/                  # Electron app template
│   ├── tauri/                     # Tauri app template
│   ├── wpf/                       # WPF app template
│   └── README.md
│
├── tools/                         # CLI tools & utilities
│   ├── ci/                        # CI/CD utilities
│   ├── cli/                       # Command-line tools
│   ├── generators/                # Code generators
│   ├── scripts/                   # Common scripts
│   └── README.md
│
├── shared/                        # Shared code & libraries
│   ├── components/                # Reusable UI components
│   ├── config/                    # Shared configuration
│   ├── types/                     # TypeScript definitions
│   │   └── src/index.ts
│   ├── utils/                     # Utility functions
│   └── README.md
│
├── docs/                          # Documentation
│   ├── README.md
│   ├── adr/                       # Architecture Decision Records
│   │   ├── 001-monorepo-structure.md
│   │   ├── template.md
│   │   └── README.md
│   ├── architecture/              # Architecture guides
│   ├── getting-started/           # Onboarding
│   │   ├── README.md
│   │   └── environment.md
│   └── guides/
│       └── deployment.md
│
├── .env.example                   # Environment template
├── .editorconfig                  # Editor settings
├── eslint.config.js               # ESLint flat config
├── .prettierrc.json               # Prettier configuration
├── .gitattributes                 # Git settings
├── .gitignore                     # Git ignore rules
├── Dockerfile                     # Production container
├── docker-compose.yml             # Local services
├── Makefile                       # Common commands
├── tsconfig.json                  # Root TypeScript config
├── tsconfig.base.json             # Base TypeScript config
├── package.json                   # Root dependencies & workspaces
├── package-lock.json              # Locked versions
├── renovate.json                  # Dependency updates
├── ARCHITECTURE.md                # Architecture overview
├── CLAUDE.md                      # Claude development guidelines
├── CONTRIBUTING.md                # Contribution guidelines
├── SECURITY.md                    # Security policy & best practices
├── SETUP.md                       # Detailed setup instructions
├── PROJECT_TYPES.md               # Guide for choosing project type
├── LICENSE                        # MIT License
└── README.md                      # This file
```

## 📚 Documentation

### Getting Started

- [Quick Start Guide](./docs/getting-started/README.md) - Get up and running in 5 minutes
- [Environment Setup](./docs/getting-started/environment.md) - Node.js, PostgreSQL, Docker configuration
- [Setup Instructions](./SETUP.md) - Detailed step-by-step installation

### Architecture & Design

- [Architecture Guide](./ARCHITECTURE.md) - Design principles and patterns
- [Architecture Decisions (ADRs)](./docs/adr/) - Technical decisions documented
  - [ADR-001: Monorepo Structure](./docs/adr/001-monorepo-structure.md)
  - [ADR Template](./docs/adr/template.md)

### Development

- [Development Guidelines](./.github/docs/README.md) - Code standards, conventions, practices
- [Programming Skills](./.github/SKILLS.md) - Patterns, best practices, design patterns
- [Contributing Guide](./CONTRIBUTING.md) - How to contribute and submit PRs
- [Prompt Templates](./.github/prompts/) - GitHub Copilot optimized prompts
- [Skill Guides](./.github/skills/) - Domain-specific development guides
- [Claude Commands](./.claude/commands/) - Claude slash-style commands for development tasks
- [Claude Guidelines](./CLAUDE.md) - Development standards written with Claude in mind

### Deployment & Operations

- [Deployment Guide](./docs/guides/deployment.md) - Deploy to Vercel, AWS, Railway, Docker
- [Security Policy](./SECURITY.md) - Security best practices and vulnerability reporting
- [Project Types Guide](./PROJECT_TYPES.md) - Choose the right project type for your needs

### Project Guides

- [Web Frontend](./apps/web-frontend/) - React + Vite frontend setup
- [Web Backend](./apps/web-backend/) - Express backend setup
- [Backend Services](./apps/README.md) - Microservices and API structure
- [Desktop Applications](./desktop/README.md) - Electron and Tauri templates
- [CLI Tools](./tools/README.md) - Command-line utilities and scripts
- [Shared Libraries](./shared/README.md) - Types, utilities, and components

## 🤖 With GitHub Copilot

This template is optimized for GitHub Copilot with pre-written prompts and skill guides:

1. **Select your task type:**
   - Feature implementation → `.github/prompts/01-feature-implementation.md`
   - Bug fixing → `.github/prompts/02-bug-fixing.md`
   - Refactoring → `.github/prompts/03-refactoring.md`
   - API design → `.github/prompts/04-api-design.md`
   - Testing → `.github/prompts/05-testing-strategy.md`
   - Performance → `.github/prompts/06-performance-optimization.md`

2. **Reference domain-specific guide:**
   - Web development → `.github/skills/01-web-development.md`
   - Backend → `.github/skills/02-backend-development.md`
   - Desktop → `.github/skills/03-desktop-development.md`
   - Tools → `.github/skills/04-tools-development.md`
   - Documentation → `.github/skills/05-documentation-development.md`
   - Shared libraries → `.github/skills/06-shared-development.md`

3. **Use Copilot Chat with structured prompts:**

   ```
   @copilot Follow the pattern in .github/prompts/01-feature-implementation.md

   Context: [Your context]
   Requirements: [What to build]
   Constraints: [Tech stack, patterns to follow]
   Include: Error handling, types, tests, documentation
   ```

4. **Verify & commit:**
   ```bash
   npm run lint && npm run test && npm run type-check
   git commit -m "feat(scope): description"
   ```

### Example Workflow: Add User Service

```bash
# 1. Read the template
cat .github/prompts/01-feature-implementation.md

# 2. Read patterns
cat .github/skills/02-backend-development.md

# 3. Use Copilot (copy the prompt template, customize with your context)

# 4. Test & verify
npm run test -w @base-template/web-backend
npm run lint:fix
npm run type-check

# 5. Commit
git commit -m "feat(backend): add user service with validation and error handling"
```

## 🧠 With Claude

This repository includes guidance and commands tailored for Claude users.

- **Guidelines:** See [CLAUDE.md](CLAUDE.md) for development standards written with Claude in mind.
- **Interactive commands:** The `.claude/commands/` folder contains slash-style prompts (e.g. `feature.md`, `api.md`, `bug.md`) that Claude can use to provide contextual, repo-aware assistance.
- **How to use:** Open the files under `.claude/commands` to see available commands (e.g. `/project:feature`, `/project:api`, `/project:backend`) and paste them into Claude chat or use your Claude integration to run them against the repo.

If you use both GitHub Copilot and Claude, prefer Copilot for inline code suggestions and Claude for higher-level planning and multi-step tasks.

## 💡 Development Workflow

### Example: Implementing a Feature

```
1. Read: .github/prompts/01-feature-implementation.md
2. Reference: .github/SKILLS.md (error handling, types)
3. Use Copilot or Claude:
   "Create a [Feature] service following this pattern: [Template]
    Include: Error handling, types, unit tests, JSDoc"
4. Test: npm run test
5. Review: Check against code standards
```

## 🛠️ Technology Stack

### Frontend

| Technology     | Version | Purpose           |
| -------------- | ------- | ----------------- |
| **React**      | 18+     | UI Framework      |
| **TypeScript** | 5.0+    | Type Safety       |
| **Vite**       | 6.0+    | Build Tool        |
| **Vitest**     | 4.0+    | Testing Framework |
| **Zustand**    | 4.4+    | State Management  |
| **Axios**      | 1.6+    | HTTP Client       |

### Backend

| Technology     | Version | Purpose        |
| -------------- | ------- | -------------- |
| **Express**    | 4.18+   | Web Framework  |
| **TypeScript** | 5.0+    | Type Safety    |
| **PostgreSQL** | 14+     | Database       |
| **Redis**      | 7.0+    | Caching        |
| **Pino**       | 8.17+   | Logging        |
| **JWT**        | 9.1+    | Authentication |

### DevOps & Tools

| Tool               | Version | Purpose          |
| ------------------ | ------- | ---------------- |
| **Node.js**        | 18.0+   | Runtime          |
| **npm**            | 9.0+    | Package Manager  |
| **Docker**         | 24.0+   | Containerization |
| **GitHub Actions** | -       | CI/CD            |
| **ESLint**         | 9.0+    | Linting          |
| **Prettier**       | 3.0+    | Formatting       |

### Optional Technologies

**For Different Project Types:**

- Vue / Svelte (alternative frontend frameworks)
- NestJS / FastAPI (alternative backends)
- Electron / Tauri (desktop applications)
- Prisma / TypeORM (database ORM)
- GraphQL (alternative API style)

## 📋 Code Standards & Quality

### Naming Conventions

```typescript
// Variables & Functions: camelCase
const userName = 'John Doe';
function calculateTotal() {}

// Classes, Types, Interfaces: PascalCase
class UserService {}
interface IUserProfile {}
type TUserRole = 'admin' | 'user';

// Constants: UPPER_SNAKE_CASE
const MAX_RETRIES = 3;
const DEFAULT_TIMEOUT = 5000;

// Files & Directories: kebab-case
src / user - service.ts;
components / user - profile.tsx;
```

### Quality Standards

- ✅ **TypeScript**: Strict mode enabled globally
- ✅ **Testing**: Minimum 80% coverage for business logic
- ✅ **Linting**: ESLint 9 flat config with TypeScript support
- ✅ **Formatting**: Prettier with consistent rules
- ✅ **Type Safety**: Full type annotations required
- ✅ **Error Handling**: Try-catch blocks everywhere
- ✅ **Documentation**: JSDoc on public APIs

### Pre-Commit Checks

```bash
# Automatically run before commit (via npm run precommit):
npm run lint         # Check for issues
npm run type-check   # Verify types
npm run format:check # Verify formatting

# Or fix issues automatically:
npm run lint:fix     # Fix linting issues
npm run format       # Format code
```

### Code Review Checklist

- [ ] Follows naming conventions
- [ ] Type-safe (strict TypeScript)
- [ ] Error handling complete
- [ ] Tests pass with 80%+ coverage
- [ ] No code duplication
- [ ] Security implications reviewed
- [ ] Performance considered
- [ ] Documentation updated
- [ ] Changelog entry added (if applicable)

## 📦 Monorepo Workspace Structure

This template uses **npm Workspaces** for efficient monorepo management:

```bash
# Install all dependencies across all workspaces
npm install

# Run scripts in all workspaces
npm run dev --workspaces
npm run build --workspaces
npm run test --workspaces

# Run scripts in specific workspace
npm run dev -w @base-template/web-frontend
npm run test -w @base-template/web-backend
npm run lint -w @base-template/api
```

### Workspace Configuration

```json
{
  "workspaces": ["apps/*", "desktop/*", "tools/*", "shared/*"]
}
```

**Benefits:**

- 📦 Unified dependency management
- 🔄 Efficient link packages for shared code
- 🚀 Single build process
- 📊 Shared TypeScript/ESLint configuration
- 🔐 Consistent code quality across projects

## 🔐 Security Features

This template implements production-grade security:

### Built-In Security

- 🔐 **JWT Authentication** - Secure token-based auth with expiry
- 🛡️ **Helmet.js** - HTTP security headers
- 🚫 **CORS Configuration** - Controlled cross-origin requests
- ⏱️ **Rate Limiting** - Prevent abuse and DDoS
- 🔍 **Input Validation** - Joi for request validation
- 📝 **Environment Variables** - Secrets management
- 🔒 **SQL Injection Prevention** - Parameterized queries
- 🛡️ **XSS Protection** - Input sanitization

### GitHub Security Features

- 🔄 **Dependabot** - Automated dependency updates (via renovate.json)
- 🔍 **Security Scanning** - Trivy vulnerability scan
- 📋 **SECURITY.md** - Responsible disclosure policy
- 🚨 **Issue Templates** - Security report guidance

See [Security Policy](./SECURITY.md) for comprehensive guidelines.

## 🧪 Testing & Quality Assurance

### Built-In Test Setup

```bash
# Run all tests
npm run test

# Watch mode (re-run on changes)
npm run test:watch

# Generate coverage report
npm run test:coverage
```

### Test Strategy

- **Unit Tests**: Individual function/component testing
- **Integration Tests**: Module interaction testing
- **E2E Tests**: User workflow testing (optional, add Cypress/Playwright)
- **Coverage Goal**: Minimum 80% for business logic

### Code Quality Tools

| Tool           | Purpose       | Config             |
| -------------- | ------------- | ------------------ |
| **ESLint**     | Linting       | `eslint.config.js` |
| **Prettier**   | Formatting    | `.prettierrc.json` |
| **TypeScript** | Type checking | `tsconfig.json`    |
| **Vitest**     | Unit testing  | `vitest.config.ts` |

### Automated Quality Checks

GitHub Actions runs on every push:

- ✅ Lint code
- ✅ Run tests
- ✅ Type checking
- ✅ Build verification
- ✅ Security scanning

## 🚀 Deployment Guide

### Local Development with Docker

```bash
# Start PostgreSQL and Redis
docker-compose up -d

# Verify services
npm run dev
# Frontend: http://localhost:5173
# API: http://localhost:3000/api/health
```

### Cloud Deployment Options

| Platform       | Best For   | Setup Time | Cost                |
| -------------- | ---------- | ---------- | ------------------- |
| **Vercel**     | Frontend   | < 5 min    | Free tier available |
| **Railway**    | Backend    | < 10 min   | Pay-as-you-go       |
| **AWS (ECS)**  | Production | 15-30 min  | Pay-per-use         |
| **Docker Hub** | Any        | varies     | Flexible            |

### Quick Deploy

**Vercel (Frontend):**

```bash
npm install -g vercel
vercel --prod
```

**Railway (Backend):**

```bash
# Connect repository on railway.app
# Select apps/web-backend as root directory
# Deploy automatically
```

**Docker (Any Platform):**

```bash
docker build -t my-app .
docker run -p 3000:3000 my-app
```

See [Deployment Guide](./docs/guides/deployment.md) for detailed instructions.

## 🤝 Contributing

### Contribution Process

1. **Fork & Clone**

   ```bash
   git clone https://github.com/WalSplitter/base-project-template.git
   cd base-project-template
   npm install
   ```

2. **Create Feature Branch**

   ```bash
   git checkout -b feature/my-feature
   ```

3. **Follow Development Standards**
   - Read [Development Guidelines](./.github/copilot-instructions.md)
   - Use [Code Standards](#-code-standards--quality)
   - Write tests for new features

4. **Commit with Conventional Format**

   ```bash
   # Format: type(scope): subject
   git commit -m "feat(auth): add JWT token refresh"
   git commit -m "fix(api): handle null user gracefully"
   git commit -m "docs(readme): update setup instructions"
   ```

   **Types:** `feat` | `fix` | `docs` | `style` | `refactor` | `perf` | `test` | `chore`

5. **Run Quality Checks**

   ```bash
   npm run lint:fix
   npm run type-check
   npm run test
   npm run build
   ```

6. **Create Pull Request**
   - Use [PR Template](./.github/PULL_REQUEST_TEMPLATE.md)
   - Reference related issues
   - Describe changes clearly

7. **Address Review Feedback**

8. **Merge & Celebrate**

### Reporting Issues

**Security Issues:**

- Email: security@example.com (DO NOT create public issue)
- See [Security Policy](./SECURITY.md)

**Bug Reports:**

- Use [Bug Template](./.github/ISSUE_TEMPLATE/bug.yml)
- Include: OS, Node version, reproduction steps

**Feature Requests:**

- Use [Feature Template](./.github/ISSUE_TEMPLATE/feature.yml)
- Explain use case and expected behavior

## ❓ FAQ & Troubleshooting

### Common Questions

**Q: Can I use this for production?**
A: Yes! This template is designed for production applications with security, testing, and deployment best practices built-in.

**Q: Do I need all project types?**
A: No. Remove unused directories (desktop/, tools/, etc.) and their workspace entries in `package.json`.

**Q: How do I switch technologies?**
A: Adapt the template to your stack:

- Replace React with Vue/Svelte in `apps/web-frontend`
- Replace Express with NestJS/FastAPI in `apps/web-backend`
- Use your preferred database (PostgreSQL is recommended)

**Q: Can I use this with a different package manager?**
A: Yes, but npm workspaces are recommended. Yarn and pnpm also support workspaces.

**Q: How do I update shared types across projects?**
A: Changes to `shared/types/src/` are automatically used by all projects through npm workspaces.

### Troubleshooting

**Problem: `npm install` fails**

```bash
# Clear npm cache
npm cache clean --force
# Reinstall
rm -rf node_modules package-lock.json
npm install
```

**Problem: Port already in use**

```bash
# Find process on port 3000
lsof -i :3000
# Kill process
kill -9 <PID>
```

**Problem: TypeScript errors**

```bash
# Ensure all tsconfig files are present
npm run type-check --workspaces
# If errors persist, check "extends" paths in tsconfig.json
```

**Problem: Database connection fails**

```bash
# Check PostgreSQL is running
docker-compose ps
# Restart services
docker-compose restart postgres
```

**Problem: GitHub Actions failing**

- Check logs in GitHub Actions tab
- Verify environment variables in repository settings
- Ensure Node version matches `package.json` engines field

See [Troubleshooting Guide](./docs/getting-started/environment.md#troubleshooting) for more solutions.

## 🎯 Roadmap

### Current (v1.0)

- ✅ Monorepo with npm workspaces
- ✅ Full-stack web app (`apps/web-frontend` + `apps/web-backend`)
- ✅ Standalone API service template (`apps/api`)
- ✅ GitHub Actions CI/CD
- ✅ Docker support
- ✅ Comprehensive documentation
- ✅ Security guidelines
- ✅ GitHub Copilot optimization
- ✅ Claude commands & guidelines

### Planned Enhancements

- 🔄 Example applications
- 📱 Mobile backend template
- 🎨 Storybook integration
- 🔗 API documentation (Swagger/OpenAPI)
- 📊 Monitoring & logging setup
- 🎓 Video tutorials
- 🧑‍💻 Community examples

## 📞 Support & Community

### Resources

- 📖 **[Full Documentation](./docs/README.md)** - Comprehensive guides
- 🐛 **[GitHub Issues](../../issues)** - Bug reports and feature requests
- 💬 **[GitHub Discussions](../../discussions)** - Questions and ideas
- 🔐 **[Security Policy](./SECURITY.md)** - Report vulnerabilities responsibly

### Getting Help

1. **Check Documentation** - Many questions are answered in guides
2. **Search Issues** - Your problem might already be solved
3. **Ask in Discussions** - Community can help
4. **File Issue** - If it's a bug, report it

## 📝 License & Attribution

This project is licensed under the **MIT License**. See [LICENSE](LICENSE) for details.

### License Summary

✅ **Allowed:** Commercial use, Modification, Distribution, Private use

❌ **Not Allowed:** No warranty or liability

### Attribution

While not required, we appreciate credit to [WalSplitter](https://github.com/WalSplitter/) and linking to this repository.

---

## 🎉 Getting Started Now!

1. **Clone or use as template** → Start with your project
2. **Follow quick start** → Up and running in 5 minutes
3. **Read guidelines** → Understand development standards
4. **Start coding** → Build your amazing application

### Next Steps

- 📖 Read [Quick Start Guide](./docs/getting-started/README.md)
- 🤖 Explore [GitHub Copilot Prompts](./.github/prompts/)
- 🧠 Explore [Claude Commands](./.claude/commands/)
- 🏗️ Review [Architecture Decisions](./docs/adr/)
- 🚀 Deploy your first application

---

**Template Version:** 1.0  
**Last Updated:** 2026-06-04  
**Maintained by:** WalSplitter  
**License:** MIT

**Questions? Issues? Ideas?**  
→ Create an [Issue](../../issues) or [Discussion](../../discussions)

**Happy Coding! 🚀**
