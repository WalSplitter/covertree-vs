# Environment Setup & Development Environment

## System Requirements

### Minimum Requirements

- **OS**: Windows 10+, macOS 10.15+, or Linux (Ubuntu 20.04+)
- **Memory**: 8GB RAM
- **Disk**: 10GB free space
- **Internet**: Required for dependency installation

### Recommended Requirements

- **Memory**: 16GB RAM
- **Disk**: SSD with 20GB free space
- **Processor**: Modern multi-core CPU (Intel i5+, Apple M1+)

## Software Installation

### Node.js & npm

1. **Download**: https://nodejs.org/ (LTS version 18.0.0+)
2. **Install**: Follow the installer for your OS
3. **Verify**:
   ```bash
   node --version  # Should be v18+
   npm --version   # Should be 9.0+
   ```

### Git

1. **Download**: https://git-scm.com/
2. **Install**: Follow the installer
3. **Configure**:
   ```bash
   git config --global user.name "Your Name"
   git config --global user.email "you@example.com"
   ```

### VS Code (Recommended)

1. **Download**: https://code.visualstudio.com/
2. **Install**: Follow the installer
3. **Extensions**: Install recommended extensions

### Docker (Optional)

For database and service management:

1. **Download**: https://www.docker.com/products/docker-desktop/
2. **Install**: Follow the installer
3. **Verify**: `docker --version`

### PostgreSQL (Optional)

For local development with PostgreSQL:

**Option 1: Docker (Recommended)**

```bash
docker run --name postgres \
  -e POSTGRES_PASSWORD=password \
  -p 5432:5432 \
  -d postgres:latest
```

**Option 2: Direct Installation**

- **Windows**: Download from https://www.postgresql.org/download/windows/
- **macOS**: Use Homebrew: `brew install postgresql`
- **Linux**: `sudo apt-get install postgresql`

## IDE Setup

### VS Code Configuration

#### 1. Recommended Extensions

```json
{
  "recommendations": [
    "dbaeumer.vscode-eslint",
    "esbenp.prettier-vscode",
    "mhutchie.git-graph",
    "eamodio.gitlens",
    "ms-vscode.vscode-typescript-next",
    "zhuangtongfa.material-theme",
    "ms-vscode.makefile-tools"
  ]
}
```

Install from `.vscode/extensions.json`

#### 2. VS Code Settings

Create `.vscode/settings.json`:

```json
{
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  "editor.formatOnSave": true,
  "editor.codeActionsOnSave": {
    "source.fixAll.eslint": true
  },
  "typescript.tsdk": "node_modules/typescript/lib",
  "typescript.enablePromptUseWorkspaceTsdk": true,
  "files.exclude": {
    "**/node_modules": true,
    "**/.turbo": true,
    "**/dist": true
  }
}
```

#### 3. Debug Configuration

Create `.vscode/launch.json`:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Backend",
      "type": "node",
      "request": "launch",
      "program": "${workspaceFolder}/apps/web-backend/src/index.ts",
      "preLaunchTask": "npm: dev",
      "skipFiles": ["<node_internals>/**"]
    }
  ]
}
```

### IntelliJ IDEA / WebStorm

1. **ESLint**: Settings > Languages & Frameworks > JavaScript > Code Quality Tools > ESLint
2. **Prettier**: Settings > Languages & Frameworks > JavaScript > Prettier
3. **TypeScript**: Settings > Languages & Frameworks > TypeScript

## Database Setup

### PostgreSQL Local Development

#### 1. Using Docker (Recommended)

```bash
# Start PostgreSQL container
docker run --name dev-postgres \
  -e POSTGRES_USER=developer \
  -e POSTGRES_PASSWORD=password \
  -e POSTGRES_DB=dev_db \
  -p 5432:5432 \
  -v pgdata:/var/lib/postgresql/data \
  -d postgres:latest

# Connect
psql -U developer -h localhost -d dev_db
```

#### 2. Using Local Installation

```bash
# macOS
brew install postgresql
brew services start postgresql

# Linux
sudo apt-get install postgresql
sudo systemctl start postgresql

# Windows: Use GUI installer
```

#### 3. Create Database

```bash
createdb -U postgres dev_db

# Or via psql
psql -U postgres
CREATE DATABASE dev_db;
```

#### 4. Connection String

```
postgresql://developer:password@localhost:5432/dev_db
```

## Environment Variables

### Development Environment

Create `.env` in project root:

```env
# Application
NODE_ENV=development
PORT=3000
LOG_LEVEL=debug

# Database
DATABASE_URL=postgresql://developer:password@localhost:5432/dev_db
DATABASE_POOL_SIZE=10

# JWT
JWT_SECRET=your-dev-secret-change-in-production
JWT_EXPIRY=24h

# Frontend
VITE_API_BASE_URL=http://localhost:3000
VITE_APP_NAME=Dev App
```

### Production Environment

Set via environment or deployment platform:

```env
NODE_ENV=production
PORT=3000
DATABASE_URL=postgresql://user:pass@prod-db:5432/prod_db
JWT_SECRET=<secure-random-secret>
```

## Useful Development Tools

### Command Line Tools

```bash
# Process management
brew install htop          # System monitoring
npm install -g pm2         # Process manager
npm install -g concurrently # Run multiple commands

# Database tools
npm install -g pgcli       # PostgreSQL CLI client
npm install -g sql-cli     # SQL CLI

# Development tools
npm install -g nodemon     # Auto-restart on changes
npm install -g tsx         # TypeScript execute
```

### Browser Extensions

**Chrome/Edge:**

- React Developer Tools
- Redux DevTools
- JSON Viewer
- REST Client

**Firefox:**

- React Developer Tools
- Redux DevTools
- JSONView

## Performance Optimization

### npm Configuration

Create `.npmrc`:

```ini
# Increase timeout for large packages
fetch-timeout=120000

# Use legacy peer deps if needed
legacy-peer-deps=false

# Increase audit timeout
audit-timeout=120000
```

### Node Configuration

For large projects, increase Node memory:

```bash
# macOS/Linux
export NODE_OPTIONS="--max-old-space-size=4096"

# Windows
set NODE_OPTIONS=--max-old-space-size=4096

# Or in .env
NODE_OPTIONS=--max-old-space-size=4096
```

## Troubleshooting

### Issue: npm install hangs

```bash
npm cache clean --force
npm install --verbose
```

### Issue: EACCES permissions error

```bash
# Don't use sudo! Fix npm permissions instead
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
export PATH=~/.npm-global/bin:$PATH
```

### Issue: Node version mismatch

```bash
# Use nvm to manage Node versions
brew install nvm
nvm install 20
nvm use 20
```

### Issue: PostgreSQL connection refused

```bash
# Check if PostgreSQL is running
# macOS
brew services list

# Linux
sudo systemctl status postgresql

# Windows: Check Services in Task Manager
```

## Next Steps

1. Complete [Project Setup](../SETUP.md)
2. Review [Development Guidelines](./.github/copilot-instructions.md)
3. Start developing!

---

Last Updated: 2026-06-01
