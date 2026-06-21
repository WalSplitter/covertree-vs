---
name: Documentation Development
description: Best practices for writing project documentation, guides, architecture docs, and developer references
keywords:
  - documentation
  - docs
  - guides
  - markdown
  - architecture
  - writing
  - technical-writing
  - knowledge-base
topics:
  - Documentation Structure
  - Markdown Standards
  - Architecture Decisions
  - API Documentation
  - Troubleshooting Guides
  - README Best Practices
applyTo:
  - /docs/**
relatedPrompts:
  - 03-refactoring.md
  - 05-testing-strategy.md
relatedSkills:
  - 01-web-development.md
  - 02-backend-development.md
  - 06-shared-development.md
version: 1.0
---

# Documentation Development

Professional guidance for creating clear, maintainable documentation, guides, and architecture references.

## Documentation Strategy

### Structure

- Keep documentation organized by purpose
- Separate user guides, architecture, API docs, and troubleshooting
- Use `docs/` for general project docs and `docs/adr/` for decisions

### Writing Style

- Use simple, direct language
- Prefer active voice and short sentences
- Keep examples concrete and reproducible
- Use consistent formatting and headings

### Markdown Best Practices

- Use headings: `#`, `##`, `###`
- Keep code blocks fenced and language-tagged
- Use bullet lists for steps and options
- Use tables for comparisons or data
- Keep line length readable (80-100 chars)

### Recommended Documents

- `README.md`: High-level project overview
- `docs/getting-started.md`: Setup and onboarding
- `docs/architecture.md`: System design and patterns
- `docs/guides/*.md`: How-to instructions
- `docs/troubleshooting.md`: Common problems and fixes
- `docs/adr/*.md`: Architecture decision records

## API Documentation

- Document each endpoint with method, path, request schema, response schema
- Explain authentication and error cases
- Include sample requests and responses
- Keep documentation aligned with actual API behavior

## Architecture Documentation

- Use diagrams for component interactions
- Describe data flow and responsibility boundaries
- Explain key decisions and trade-offs
- Keep architecture docs updated with major changes

## Troubleshooting Guides

- Capture common failure modes clearly
- Provide reproduction steps and fixes
- Link to related docs or code sections
- Keep troubleshooting concise and actionable

## Documentation Review

- Ensure docs are accurate and up to date
- Validate examples with real commands
- Verify links and references
- Use documentation as living source of truth

## Tooling Recommendations

- Use `docs/README.md` to index the documentation
- Use Mermaid diagrams for architecture visuals
- Consider static site generators like VitePress or Docusaurus

---

**Related Resources:**

- Prompts: [Refactoring](../prompts/03-refactoring.md), [Testing Strategy](../prompts/05-testing-strategy.md)
- Standards: [copilot-instructions.md](../copilot-instructions.md)
