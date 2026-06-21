# Desktop Applications

Dedicated directory for cross-platform and native desktop applications.

## Project Structure

```
desktop/
├── electron-app/              # Electron-based desktop app
│   ├── public/
│   │   └── electron.js        # Main process
│   ├── src/
│   │   ├── components/        # UI components
│   │   ├── pages/             # Pages
│   │   ├── services/          # Business logic
│   │   ├── ipc/               # IPC handlers
│   │   ├── store/             # State management
│   │   ├── types/             # TypeScript types
│   │   ├── utils/             # Utilities
│   │   └── App.tsx            # App entry
│   ├── tests/
│   ├── package.json
│   └── README.md
│
├── tauri-app/                 # Tauri-based desktop app
│   ├── src-tauri/
│   │   ├── src/
│   │   │   ├── main.rs        # Main Rust code
│   │   │   └── lib.rs
│   │   ├── tauri.conf.json
│   │   └── Cargo.toml
│   ├── src/
│   │   ├── components/        # React components
│   │   ├── pages/
│   │   ├── services/
│   │   ├── types/
│   │   └── App.tsx
│   ├── tests/
│   └── package.json
│
├── wpf-app/                   # WPF desktop application (.NET)
│   ├── Views/
│   ├── ViewModels/
│   ├── Models/
│   ├── Services/
│   ├── App.xaml
│   ├── App.xaml.cs
│   └── *.csproj
│
└── macos-app/                 # Native macOS app (Swift)
    ├── Sources/
    ├── Resources/
    ├── Tests/
    └── Package.swift
```

## Technology Recommendations

### Electron (TypeScript + React)

```bash
# Setup
npx create-electron-app my-app --template webpack --template-type webpack-typescript

# Key Packages
npm install electron-squirrel-startup
npm install -D electron electron-builder

# Best for: Cross-platform (Windows, macOS, Linux)
# Pros: JavaScript/TypeScript, React ecosystem, easy development
# Cons: Larger app size, higher memory usage
```

**Architecture:**

```
Main Process (Node.js)
    ↓
IPC (Inter-Process Communication)
    ↓
Renderer Process (Chromium/Browser)
```

### Tauri (Rust + React/Vue)

```bash
# Setup
npm create tauri-app@latest

# Best for: Cross-platform with smaller footprint
# Pros: Smaller bundle, native performance, Rust backend
# Cons: Steeper learning curve
```

### WPF (.NET / C#)

```bash
# Setup
dotnet new wpf -n my-app

# Best for: Windows-only applications
# Pros: Native Windows integration, .NET ecosystem
# Cons: Windows only, XAML learning curve
```

### macOS (SwiftUI)

```bash
# Best for: Native macOS applications
# Pros: Native performance, system integration
# Cons: macOS only, Swift learning curve
```

## Common Patterns

### IPC Communication (Electron)

```typescript
// Main Process (main.ts)
import { ipcMain } from 'electron';

ipcMain.handle('file:save', async (event, content: string) => {
  try {
    await fs.writeFile('file.txt', content);
    return { success: true };
  } catch (error) {
    return { success: false, error: (error as Error).message };
  }
});

ipcMain.on('app:quit', () => {
  app.quit();
});

// Renderer Process (React Component)
import { ipcRenderer } from 'electron';

function Editor() {
  const handleSave = async (content: string) => {
    const result = await ipcRenderer.invoke('file:save', content);
    if (result.success) {
      console.log('File saved');
    }
  };

  return (
    <button onClick={() => handleSave('content')}>
      Save File
    </button>
  );
}
```

### Menu Setup (Electron)

```typescript
import { Menu, MenuItem, MenuItemConstructorOptions } from 'electron';

const template: MenuItemConstructorOptions[] = [
  {
    label: 'File',
    submenu: [
      {
        label: 'Exit',
        accelerator: 'CmdOrCtrl+Q',
        click: () => app.quit(),
      },
    ],
  },
  {
    label: 'Edit',
    submenu: [
      { label: 'Undo', accelerator: 'CmdOrCtrl+Z' },
      { label: 'Redo', accelerator: 'CmdOrCtrl+Y' },
      { type: 'separator' },
      { label: 'Cut', accelerator: 'CmdOrCtrl+X' },
    ],
  },
];

const menu = Menu.buildFromTemplate(template);
Menu.setApplicationMenu(menu);
```

### File Operations

```typescript
// Main Process
import { dialog, ipcMain } from 'electron';
import * as fs from 'fs/promises';
import * as path from 'path';

ipcMain.handle('file:open-dialog', async () => {
  const result = await dialog.showOpenDialog({
    properties: ['openFile'],
    filters: [
      { name: 'Text Files', extensions: ['txt', 'md'] },
      { name: 'All Files', extensions: ['*'] },
    ],
  });

  if (!result.canceled) {
    const content = await fs.readFile(result.filePaths[0], 'utf-8');
    return { success: true, content, path: result.filePaths[0] };
  }

  return { success: false };
});
```

### System Tray (Electron)

```typescript
import { Tray, Menu } from 'electron';
import path from 'path';

const tray = new Tray(path.join(__dirname, 'assets', 'icon.png'));

const contextMenu = Menu.buildFromTemplate([
  {
    label: 'Show',
    click: () => mainWindow.show(),
  },
  {
    label: 'Exit',
    click: () => app.quit(),
  },
]);

tray.setContextMenu(contextMenu);
```

### State Persistence

```typescript
// Store user preferences using electron-store
import Store from 'electron-store';

interface IStoreSchema {
  windowBounds: { width: number; height: number };
  theme: 'light' | 'dark';
  recentFiles: string[];
}

const store = new Store<IStoreSchema>({
  defaults: {
    windowBounds: { width: 800, height: 600 },
    theme: 'light',
    recentFiles: [],
  },
});

// Usage
store.set('theme', 'dark');
const bounds = store.get('windowBounds');
```

## Getting Started

### Electron

```bash
# Create new project
npx create-electron-app my-app --template webpack

# Install dependencies
npm install

# Start development
npm run start

# Build for distribution
npm run make
```

### Tauri

```bash
# Create new project
npm create tauri-app@latest

# Install dependencies
npm install

# Start development
npm run tauri dev

# Build for production
npm run tauri build
```

## Testing

```typescript
// Electron Main Process Testing
import { test, expect } from 'vitest';
import { ipcMain } from 'electron';

test('save file handler', async () => {
  const result = await ipcMain.invoke('file:save', 'test content');
  expect(result.success).toBe(true);
});

// UI Component Testing (React Testing Library)
import { render, screen } from '@testing-library/react';
import { Editor } from './Editor';

test('renders editor', () => {
  render(<Editor />);
  expect(screen.getByText('Save')).toBeInTheDocument();
});
```

## Building & Distribution

### Electron Auto-Update

```typescript
import { autoUpdater } from 'electron-updater';

autoUpdater.checkForUpdatesAndNotify();
```

### Code Signing (macOS)

```bash
# Build with signing
electron-builder \
  --publish never \
  --sign "Developer ID Application: Your Name"
```

### Notarization (macOS)

```bash
# Requirements
export APPLE_ID="your-email@example.com"
export APPLE_APP_SPECIFIC_PASSWORD="xxxx-xxxx-xxxx-xxxx"
```

## Performance Tips

- **Lazy load modules**: Load only what's needed
- **Code splitting**: Split large modules
- **Native modules**: Use native bindings for heavy computations
- **Memory management**: Monitor and clean up resources
- **V8 code caching**: Improve startup time

## Security Best Practices

- **Disable nodeIntegration**: Set to false in webPreferences
- **Enable sandbox**: Isolate renderer process
- **Validate IPC messages**: Always validate data from renderer
- **Update dependencies**: Keep Electron and packages updated
- **Code signing**: Sign and notarize releases

---

**Last Updated**: 2026  
**Version**: 1.0
