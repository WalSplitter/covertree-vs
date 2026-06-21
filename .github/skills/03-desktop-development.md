---
name: Desktop Application Development
description: Best practices for building cross-platform and native desktop applications using Electron, Tauri, and WPF
keywords:
  - desktop
  - electron
  - tauri
  - wpf
  - rust
  - ui
  - ipc
  - native
  - windows
  - macos
topics:
  - Desktop Architecture
  - IPC Communication
  - Native Integrations
  - Security for Desktop Apps
  - Performance Optimization
  - Packaging and Distribution
applyTo:
  - /desktop/**
relatedPrompts:
  - 01-feature-implementation.md
  - 02-bug-fixing.md
  - 03-refactoring.md
  - 06-performance-optimization.md
relatedSkills:
  - 01-web-development.md
  - 02-backend-development.md
  - 06-shared-development.md
version: 1.0
---

# Desktop Application Development

Professional guidance for building desktop applications with modern frameworks and native integration.

## Recommended Architecture

### Electron

- Main process: app lifecycle, native APIs, file system access
- Renderer process: UI, React/Vue/Svelte, DOM rendering
- IPC: use `ipcMain` and `ipcRenderer` for secure communication
- Security: disable `nodeIntegration`, enable `contextIsolation`

### Tauri

- Use Rust backend for local system access
- Keep frontend in TypeScript/React/Vue
- Secure communication through Tauri commands
- Smaller binaries and lower memory usage compared to Electron

### WPF

- Use MVVM pattern for separation of UI and logic
- Data binding for declarative UI updates
- Use `ICommand` for command handling
- Keep UI responsive by using async operations in view models

## Development Best Practices

### 1. Secure IPC Pattern

```typescript
// main.ts
ipcMain.handle('file:read', async (event, filePath) => {
  if (!isSafePath(filePath)) throw new Error('Invalid path');
  return fs.readFile(filePath, 'utf-8');
});

// renderer.tsx
const content = await ipcRenderer.invoke('file:read', path);
```

### 2. File Operations with Validation

- Validate input paths before reading/writing
- Use a dedicated `FileService` in the main process
- Return standardized error objects

### 3. Native Menus and Shortcuts

- Implement custom menus for desktop usability
- Include platform-specific shortcuts
- Keep menu actions separate from UI logic

### 4. Offline Support and Local Storage

- Use `electron-store`, SQLite, or indexed database
- Persist window state, theme, and recent files
- Provide recovery for unsaved work

## Packaging and Distribution

- Use `electron-builder` or `tauri build`
- Configure platform-specific signing keys
- Generate installers for Windows, macOS, Linux
- Test distribution packages on target platforms

## Performance Recommendations

- Use lazy loading for heavy UI routes
- Avoid blocking the main process
- Optimize renderer assets and bundle size
- Use native threads for CPU-heavy work when needed

## Testing Desktop Apps

- Unit test UI logic with Jest/Vitest
- Integration test IPC handlers
- End-to-end test application flows with Playwright or Spectron

## Key Principles

- Keep desktop features isolated from web-specific logic
- Use strong typing and explicit contracts for IPC messages
- Validate all external inputs from the renderer
- Document app behavior and platform requirements

---

**Related Resources:**

- Prompts: [Feature Implementation](../prompts/01-feature-implementation.md), [Bug Fixing](../prompts/02-bug-fixing.md)
- Standards: [copilot-instructions.md](../copilot-instructions.md)
