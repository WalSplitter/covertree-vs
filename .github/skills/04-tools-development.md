---
name: Tools & CLI Development
description: Best practices for building command line tools, automation scripts, code generators, and developer utilities
keywords:
  - cli
  - tools
  - automation
  - commander
  - yargs
  - scripts
  - generator
  - nodejs
  - python
  - workflow
topics:
  - CLI Architecture
  - Command Parsers
  - Automation Scripts
  - File Processing
  - Deployment Utilities
  - Testing CLI Tools
applyTo:
  - /tools/**
relatedPrompts:
  - 01-feature-implementation.md
  - 02-bug-fixing.md
  - 03-refactoring.md
  - 05-testing-strategy.md
relatedSkills:
  - 02-backend-development.md
  - 06-shared-development.md
  - 05-documentation-development.md
version: 1.0
---

# Tools & CLI Development

Guidance for creating reliable CLI tools, automation pipelines, and utility scripts with best practices for developer workflows.

## Recommended CLI Architecture

### Command Definitions

- Use Commander.js or Yargs for command parsing
- Keep each command small and focused
- Provide clear help text and examples
- Support `--help` and `--version`

### Input Validation

- Validate command arguments and flags
- Provide clear error messages
- Use a consistent response format for CLI output

### Modular Command Handlers

```typescript
const program = new Command();
program
  .command('generate <type> <name>')
  .description('Generate a new resource')
  .option('-d, --directory <path>', 'Output directory')
  .action(async (type, name, options) => {
    await generateResource(type, name, options.directory);
  });
```

## Common Utility Patterns

### Build Automation

- Create reusable build tasks
- Encapsulate file operations in services
- Generate artifacts in a clean output directory
- Support dry-run mode for safe execution

### Code Generation

- Use templates for files and code structure
- Parameterize names and paths
- Validate generated output
- Add safety checks to avoid overwriting files unintentionally

### Data Processing

- Use streaming and batching for large datasets
- Validate input formats before parsing
- Provide progress feedback when appropriate
- Write idempotent transformations

## Testing CLI Tools

- Test command parsing and error handling
- Use snapshot tests for generated output
- Mock file system interactions when appropriate
- Run integration tests with actual command process execution

## Distribution and Packaging

- Support installable CLI binaries if needed
- Use NPM package `bin` entries for Node tools
- Document usage in README
- Publish stable versions with semantic versioning

## Developer Workflow

- Keep tools easy to extend and maintain
- Document command options clearly
- Use logging only for debugging, not for normal output
- Include examples for typical use cases

---

**Related Resources:**

- Prompts: [Feature Implementation](../prompts/01-feature-implementation.md), [Testing Strategy](../prompts/05-testing-strategy.md)
- Standards: [copilot-instructions.md](../copilot-instructions.md)
