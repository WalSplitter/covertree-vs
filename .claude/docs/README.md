# Claude integration for this repository

This folder contains configuration and command templates to help Claude provide repository-aware guidance.

Files:

- `settings.json` — allowed/denied shell commands for Claude integrations.
- `commands/` — a set of markdown command templates (quick-start, feature, api, tests, etc.) that can be pasted into Claude chat or used by your Claude tooling.

How to use:

1. Open a Claude chat instance (local or cloud).
2. Copy a command template from `.claude/commands`, fill any `$ARGUMENTS` placeholders, and paste it into Claude.
3. Use the suggested steps or ask Claude to produce code snippets, tests, and commit messages.

Best practices:

- Use Copilot for inline code completions and Claude for higher-level planning and multi-step tasks.
- Keep `$ARGUMENTS` concise and provide workspace path (e.g. `apps/web-backend`) when relevant.
- Review and test generated code locally before committing.

If you want changes to these templates for a specific Claude setup, open an issue or submit a PR with suggested edits.

---

Date: 2026
