# CLI Tools & Automation Patterns

Domain-specific guidance for the `/tools/*` workspace.

## CLI Architecture

```
tools/<tool-name>/
├── src/
│   ├── index.ts        ← Entry point, program setup
│   ├── commands/       ← One file per command
│   │   ├── init.ts
│   │   └── generate.ts
│   ├── utils/          ← Shared helpers
│   └── types.ts        ← CLI-specific types
├── bin/
│   └── cli.js          ← Shebang entry (#!/usr/bin/env node)
└── package.json        ← { "bin": { "<tool>": "./bin/cli.js" } }
```

## Command parser (Commander.js)

```typescript
import { Command } from 'commander';

const program = new Command();

program.name('my-tool').description('What this tool does').version('1.0.0');

program
  .command('generate <name>')
  .description('Generate a new component')
  .option('-t, --type <type>', 'Component type', 'react')
  .option('--dry-run', 'Preview without writing files')
  .action(async (name, options) => {
    await generateCommand(name, options);
  });

program.parse();
```

## UX principles for CLI

- Exit 0 on success, non-zero on error
- Print to `stdout` for data output, `stderr` for errors/logs
- Support `--dry-run` for destructive operations
- Show progress for long operations (use `ora` spinner)
- Colorize output but respect `NO_COLOR` env var

## File operations

```typescript
import { promises as fs } from 'fs';
import path from 'path';

// Always use async fs
await fs.writeFile(path.resolve(outputDir, filename), content, 'utf-8');

// Check before overwrite
const exists = await fs
  .access(filePath)
  .then(() => true)
  .catch(() => false);
if (exists && !options.force) {
  console.error(`File already exists: ${filePath}. Use --force to overwrite.`);
  process.exit(1);
}
```

## Error handling in CLI

```typescript
// Top-level error handler
process.on('uncaughtException', (error) => {
  console.error('Unexpected error:', error.message);
  process.exit(1);
});

// Command-level
try {
  await runCommand(args);
} catch (error) {
  if (error instanceof ValidationError) {
    console.error(`Error: ${error.message}`);
    process.exit(1);
  }
  throw error; // Let top-level handler catch unexpected errors
}
```

## Testing CLI tools

```bash
npm run test -w tools/<tool>
# Test commands by calling the action functions directly, not via shell
```
