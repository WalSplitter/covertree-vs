# Tools & CLI Utilities

Dedicated directory for command-line tools, scripts, and utility applications.

## Project Structure

```
tools/
├── build-automation/          # Build and automation scripts
│   ├── src/
│   │   ├── builders/         # Build logic
│   │   ├── tasks/            # Task definitions
│   │   ├── utils/
│   │   └── cli.ts            # CLI entry point
│   ├── templates/            # Build templates
│   ├── package.json
│   └── README.md
│
├── code-generator/            # Code generation tool
│   ├── src/
│   │   ├── generators/       # Generator implementations
│   │   ├── templates/        # Template engine
│   │   ├── parsers/          # Input parsers
│   │   ├── config/
│   │   └── cli.ts
│   ├── templates/
│   ├── package.json
│   └── README.md
│
├── data-processor/            # Data processing utility
│   ├── src/
│   │   ├── processors/       # Processing logic
│   │   ├── transformers/     # Data transformers
│   │   ├── validators/       # Validation logic
│   │   └── cli.ts
│   ├── data/                 # Sample data
│   ├── package.json
│   └── README.md
│
├── migration-tool/            # Database migration tool
│   ├── src/
│   │   ├── migrations/       # Migration files
│   │   ├── drivers/          # Database drivers
│   │   ├── cli.ts
│   │   └── index.ts
│   ├── package.json
│   └── README.md
│
└── test-utilities/            # Testing helper utilities
    ├── src/
    │   ├── fixtures/         # Test fixtures
    │   ├── mocks/            # Mock utilities
    │   ├── helpers/          # Helper functions
    │   └── index.ts          # Exports
    ├── package.json
    └── README.md
```

## Technology Recommendations

### CLI Framework Options

#### Commander.js (Node.js)

```bash
npm install commander
```

```typescript
import { Command } from 'commander';

const program = new Command();

program
  .command('deploy <environment>')
  .description('Deploy application')
  .option('--dry-run', 'Show what would be deployed')
  .action((environment, options) => {
    console.log(`Deploying to ${environment}`);
    if (options.dryRun) console.log('(dry run)');
  });

program.parse(process.argv);
```

#### Yargs (Node.js)

```bash
npm install yargs
```

```typescript
import yargs from 'yargs/yargs';

yargs(process.argv.slice(2))
  .command(
    'generate <type>',
    'Generate code',
    (yargs) => {
      return yargs.positional('type', {
        describe: 'Type of code to generate',
        choices: ['service', 'component', 'test'],
      });
    },
    (argv) => {
      console.log(`Generating ${argv.type}`);
    }
  )
  .parse();
```

#### Click (Python)

```bash
pip install click
```

```python
import click

@click.command()
@click.option('--name', prompt='Your name', help='Name to greet')
def hello(name):
    click.echo(f'Hello {name}!')

if __name__ == '__main__':
    hello()
```

## Common Patterns

### Build Automation

```typescript
// src/cli.ts
import { Command } from 'commander';
import * as fs from 'fs/promises';
import * as path from 'path';

const program = new Command();

program
  .command('build [target]')
  .description('Build project')
  .option('-p, --prod', 'Production build')
  .action(async (target = 'default', options) => {
    console.log(`Building ${target} (prod: ${options.prod})`);

    // Run build tasks
    const buildPath = path.resolve('dist');
    await fs.rm(buildPath, { recursive: true, force: true });
    await fs.mkdir(buildPath, { recursive: true });

    console.log('✓ Build complete');
  });

program.parse(process.argv);
```

### Code Generation

```typescript
// src/generators/service-generator.ts
import * as fs from 'fs/promises';
import * as path from 'path';

export class ServiceGenerator {
  async generate(name: string, outputDir: string): Promise<void> {
    const template = `
import { injectable } from 'tsyringe';

@injectable()
export class ${this.pascalCase(name)}Service {
  async execute(): Promise<void> {
    // Implementation
  }
}
`.trim();

    const filePath = path.join(outputDir, `${this.kebabCase(name)}.service.ts`);
    await fs.writeFile(filePath, template);
  }

  private pascalCase(str: string): string {
    return str
      .split('-')
      .map((s) => s[0].toUpperCase() + s.slice(1))
      .join('');
  }

  private kebabCase(str: string): string {
    return str.replace(/([A-Z])/g, '-$1').toLowerCase();
  }
}
```

### Data Processing

```typescript
// src/processors/csv-processor.ts
import * as fs from 'fs/promises';
import * as csv from 'fast-csv';

export class CsvProcessor {
  async parseFile(filePath: string): Promise<Record<string, unknown>[]> {
    const rows: Record<string, unknown>[] = [];

    return new Promise((resolve, reject) => {
      fs.createReadStream(filePath)
        .pipe(csv.parse({ headers: true }))
        .on('data', (row) => rows.push(row))
        .on('end', () => resolve(rows))
        .on('error', reject);
    });
  }

  async transformData(
    data: Record<string, unknown>[],
    transformer: (row: Record<string, unknown>) => Record<string, unknown>
  ): Promise<Record<string, unknown>[]> {
    return data.map(transformer);
  }

  async writeFile(data: Record<string, unknown>[], filePath: string): Promise<void> {
    return new Promise((resolve, reject) => {
      const writeStream = fs.createWriteStream(filePath);
      csv
        .write(data, { headers: true })
        .pipe(writeStream)
        .on('finish', () => resolve())
        .on('error', reject);
    });
  }
}
```

### Database Migrations

```typescript
// src/migrations/001-create-users-table.ts
import { Database } from '../database';

export class CreateUsersTable {
  async up(db: Database): Promise<void> {
    await db.exec(`
      CREATE TABLE users (
        id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
        email VARCHAR(255) NOT NULL UNIQUE,
        name VARCHAR(255) NOT NULL,
        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
      );
    `);
  }

  async down(db: Database): Promise<void> {
    await db.exec('DROP TABLE users;');
  }
}

// CLI Usage
program
  .command('migrate:up')
  .description('Run pending migrations')
  .action(async () => {
    const migration = new CreateUsersTable();
    await migration.up(db);
    console.log('✓ Migration completed');
  });
```

## Getting Started

### Create a New CLI Tool

```bash
# Initialize project
mkdir tools/my-tool
cd tools/my-tool
npm init -y

# Install dependencies
npm install commander typescript ts-node @types/node
npm install -D @types/node

# Create structure
mkdir src
touch src/cli.ts
touch package.json
```

### Basic CLI Structure

```typescript
// src/cli.ts
import { Command } from 'commander';
import { version } from '../package.json';

const program = new Command();

program.name('my-tool').description('Description of your tool').version(version);

program
  .command('do-something <target>')
  .description('Does something')
  .option('-v, --verbose', 'Verbose output')
  .action((target, options) => {
    console.log(`Doing something with ${target}`);
  });

program.parse(process.argv);
```

### Run CLI Tool

```bash
# Development
npx ts-node src/cli.ts do-something target

# Build
npm run build

# Global install (from package.json with "bin")
npm install -g .

# Then use globally
my-tool do-something target
```

## Publishing to npm

```bash
# Update package.json
{
  "name": "@yourorg/my-tool",
  "version": "1.0.0",
  "bin": {
    "my-tool": "./dist/cli.js"
  },
  "files": ["dist"]
}

# Build
npm run build

# Login and publish
npm login
npm publish
```

## Testing CLI Tools

```typescript
// tests/cli.test.ts
import { test, expect } from 'vitest';
import { execSync } from 'child_process';

test('should show version', () => {
  const output = execSync('npm run cli -- --version', {
    encoding: 'utf-8',
  });
  expect(output).toContain('1.0.0');
});

test('should execute command', () => {
  const output = execSync('npm run cli -- do-something test', {
    encoding: 'utf-8',
  });
  expect(output).toContain('Doing something with test');
});
```

---

**Last Updated**: 2026  
**Version**: 1.0
