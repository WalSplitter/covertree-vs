# Getting Started with Base Project Template

Welcome! This guide will help you get started with the Base Project Template quickly.

## 5-Minute Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/WalSplitter/base-project-template.git
cd base-project-template
```

### 2. Install Dependencies

```bash
npm install
```

### 3. Setup Environment

```bash
cp .env.example .env
```

### 4. Start Development Server

```bash
npm run dev
```

- Frontend: http://localhost:5173
- Backend API: http://localhost:3000
- Health check: http://localhost:3000/api/health

## What's Included

### ✅ Development Setup

- TypeScript configuration
- ESLint for code quality
- Prettier for code formatting
- GitHub Copilot instructions
- Professional development guidelines

### ✅ Project Structure

- Full-stack web applications (apps/web-frontend + apps/web-backend)
- Backend services (apps/)
- Desktop applications (desktop/)
- CLI tools (tools/)
- Shared libraries (shared/)
- Documentation (docs/)

### ✅ Automation

- GitHub Actions CI/CD pipelines
- Pre-commit hooks configuration
- Build and test automation
- Code quality checks

### ✅ Best Practices

- Security guidelines
- Error handling patterns
- Testing strategies
- API design patterns
- Performance optimization

## Project Types

### 🌐 Web Application

Full-stack web app with React frontend and Express backend:

```bash
npm run dev -w @base-template/web-frontend
npm run dev -w @base-template/web-backend
# or start both:
npm run dev --workspaces
```

**Technologies**: React, TypeScript, Vite, Express, PostgreSQL

### 🖥️ Backend Services

REST API and microservices:

```bash
cd apps/api
npm run dev
```

**Technologies**: Express, TypeScript, PostgreSQL, Redis

### 💻 Desktop Applications

Cross-platform desktop apps:

```bash
# Electron
cd desktop/electron
npm run dev

# Tauri
cd desktop/tauri
npm run tauri dev
```

### 🔧 CLI Tools

Command-line tools and generators:

```bash
cd tools/cli
npm run dev
```

### 📦 Shared Libraries

Shared types, utilities, and components:

```bash
cd shared/types
npm run build
```

## Next Steps

1. **Read the Documentation**
   - [Setup Guide](../SETUP.md) - Detailed installation
   - [Architecture](../ARCHITECTURE.md) - Design decisions
   - [Contributing](../CONTRIBUTING.md) - How to contribute

2. **Review Guidelines**
   - [Development Guidelines](./.github/copilot-instructions.md)
   - [Code Patterns](./.github/SKILLS.md)

3. **Try Examples**
   - Explore `apps/web-frontend/` for React components
   - Check `apps/web-backend/` for API setup
   - Browse `shared/types/` for TypeScript types

4. **Start Coding**
   - Create a feature branch: `git checkout -b feature/my-feature`
   - Follow [Contributing Guide](../CONTRIBUTING.md)
   - Commit following [Conventional Commits](https://www.conventionalcommits.org/)

## Common Commands

```bash
# Development
npm run dev              # Start all dev servers
npm run dev -w @base-template/web-frontend && npm run dev -w @base-template/web-backend     # Start only web app

# Building
npm run build           # Build all projects
npm run build -w @base-template/api   # Build specific project

# Code Quality
npm run lint            # Lint all code
npm run lint:fix        # Fix lint issues
npm run format          # Format code with Prettier
npm run type-check      # TypeScript type checking

# Testing
npm run test            # Run tests
npm run test:watch      # Run tests in watch mode
npm run test:coverage   # Generate coverage report
```

## Troubleshooting

### Issue: Port Already in Use

```bash
# Kill process on port 3000
lsof -i :3000
kill -9 <PID>
```

### Issue: Dependencies Not Installed

```bash
# Clear and reinstall
rm -rf node_modules package-lock.json
npm install
```

### Issue: Build Failures

```bash
# Clean and rebuild
npm run clean
npm install
npm run build
```

### Issue: Git Hooks Not Working

```bash
# Reinstall husky
npm run prepare
```

## Resources

- 📖 [Full Documentation](../README.md)
- 🏗️ [Architecture Guide](../ARCHITECTURE.md)
- 🤝 [Contributing Guide](../CONTRIBUTING.md)
- 🔐 [Security Policy](../SECURITY.md)
- 💻 [Development Guidelines](./.github/copilot-instructions.md)

## Getting Help

- 💬 [GitHub Discussions](https://github.com/WalSplitter/base-project-template/discussions)
- 🐛 [Report Issues](https://github.com/WalSplitter/base-project-template/issues)
- 🔒 [Security Issues](../SECURITY.md)

## Next: Choose Your Path

**Learning the Template?** → Read [Architecture](../ARCHITECTURE.md)

**Ready to Code?** → Start with [Setup Guide](../SETUP.md)

**Contributing?** → Follow [Contributing Guide](../CONTRIBUTING.md)

---

**Happy Coding! 🚀**
