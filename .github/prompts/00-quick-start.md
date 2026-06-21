# Prompt: Quick Start - Base Project Template

Quick reference for efficient development with GitHub Copilot using this production-ready template.

## 🚀 Getting Started

### 1. Initial Setup

```bash
# Clone and install
git clone https://github.com/WalSplitter/base-project-template.git
cd base-project-template
npm install

# Start development servers
npm run dev --workspaces

# Frontend: http://localhost:5173
# Backend: http://localhost:3000
```

### 2. Choose Your Project Type

```
Web Application (React + Express)      → /web
Backend Services (Node.js + TypeScript) → /apps/*
Desktop Application (Electron/Tauri)   → /desktop/*
Command-Line Tool (Node.js CLI)        → /tools/*
Shared Libraries (Types/Utils)         → /shared/*
```

### 2. Essential Copilot Commands

#### Feature Development

```
@copilot /explain [function]           # Understand existing code
@copilot /refactor                     # Improve code quality
@copilot /tests                        # Generate test cases
@copilot /doc                          # Generate documentation
```

#### Quick Patterns

- Create new service with dependency injection
- Implement error handling with try-catch
- Add validation with Zod schemas
- Create unit tests with mocks
- Setup API endpoint with error handling

### 3. Follow Template Standards

Ensure consistency across workspaces:

```bash
# Naming: camelCase (vars), PascalCase (types), UPPER_SNAKE_CASE (constants)
const userName = 'John';  ✓
interface IUser {}        ✓
const MAX_RETRIES = 3;    ✓

# Formatting (auto-fixed on commit)
npm run lint:fix

# Type safety (strict mode)
npm run type-check

# Testing (80%+ coverage target)
npm run test

# Building all workspaces
npm run build --workspaces
```

## 📝 Development Patterns

### Pattern 1: Feature Development (30 mins)

```
1. Understand requirements & code standards (.github/copilot-instructions.md)
2. Create types in shared/types or feature types.ts
3. Implement core logic with error handling
4. Write unit tests (target: 80%+ coverage)
5. Document with JSDoc @param, @returns, @throws
6. Commit using Conventional Commits format
```

Before starting, reference:

- [Code Standards](./.github/copilot-instructions.md#code-standards--quality)
- [Architecture Decisions](./docs/adr/001-monorepo-structure.md)
- Similar implementations in same workspace

### Pattern 2: Bug Investigation (20 mins)

```
1. Verify issue with clear reproduction steps
2. Check type safety (npm run type-check)
3. Locate source code in appropriate workspace
4. Analyze root cause with logs
5. Implement fix with tests
6. Verify with: npm run test -w [workspace]
```

Tools to help debug:

```bash
# Check for type errors across all workspaces
npm run type-check --workspaces

# Run tests in watch mode
npm run test:watch -w [workspace]

# Check linting for specific file
npm run lint -- src/file.ts
```

### Pattern 3: Code Review (10 mins)

```
1. Skim the changes
2. Check naming conventions
3. Verify error handling
4. Review test coverage
5. Request improvements
```

Copilot prompt:

```
Review this code for:
- SOLID principles compliance
- Type safety (strict TypeScript)
- Test coverage
- Performance issues
```

### Pattern 4: Performance Tuning (45 mins)

```
1. Identify bottleneck
2. Measure baseline
3. Apply optimization
4. Re-measure
5. Document trade-offs
```

Copilot prompt:

```
Optimize [code] for [metric]:
- Current: [baseline]
- Target: [goal]
- Constraints: [limits]

Use: [technique: caching, pagination, etc.]
```

## File Organization Quick Reference

```
app-name/
├── src/
│   ├── domain/          # Business logic
│   │   ├── models/      # Data models
│   │   └── services/    # Business services
│   ├── infrastructure/  # External integrations
│   │   ├── repositories/
│   │   ├── http-clients/
│   │   └── config/
│   ├── api/             # API layer (if applicable)
│   │   ├── routes/
│   │   ├── controllers/
│   │   └── middleware/
│   ├── ui/              # UI components (if frontend)
│   │   ├── components/
│   │   ├── pages/
│   │   └── hooks/
│   ├── shared/          # Shared utilities
│   │   ├── types.ts
│   │   ├── constants.ts
│   │   └── utils/
│   └── index.ts         # Main entry point
├── tests/
│   ├── unit/
│   ├── integration/
│   └── e2e/
├── docs/                # Project documentation
└── package.json
```

## Troubleshooting

### Copilot Generating Wrong Code?

1. **Be more specific**: Add example input/output
2. **Reference patterns**: Link to similar implementations
3. **Set constraints**: Mention tech stack and requirements
4. **Ask for iterations**: "Start simple, then optimize"

### Type Errors in Generated Code?

1. Check imports are complete
2. Verify interface definitions match
3. Review TypeScript compiler errors
4. Ask Copilot to add type safety

### Tests Not Passing?

1. Verify mocks match actual service signatures
2. Check async/await handling
3. Review error message details
4. Ask Copilot to fix specific assertion

## Best Practices

### ✓ DO

- Ask Copilot to explain before changing code
- Test generated code before committing
- Reference existing patterns in prompts
- Break large requests into smaller steps
- Review security/performance implications

### ✗ DON'T

- Accept generated code without review
- Skip unit tests for generated functions
- Ignore TypeScript warnings/errors
- Mix incompatible dependencies
- Commit without running linter/formatter

## Next Steps

1. Read [copilot-instructions.md](copilot-instructions.md) for detailed standards
2. Review [SKILLS.md](SKILLS.md) for programming patterns
3. Pick a prompt from [prompts/](prompts/) for your use case
4. Create first feature using the template
5. Share patterns back to team
