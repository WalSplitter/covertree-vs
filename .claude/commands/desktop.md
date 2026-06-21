# Desktop Application Patterns

Domain-specific guidance for the `/desktop/*` workspace (Electron, Tauri, WPF).

## Architecture by framework

### Electron

```
desktop/electron/
├── src/
│   ├── main/         ← Main process (Node.js)
│   │   ├── main.ts   ← App entry, BrowserWindow setup
│   │   └── ipc/      ← IPC handlers
│   └── renderer/     ← Renderer process (React/Vue)
│       ├── components/
│       └── preload.ts ← Secure bridge (contextBridge)
```

**Security – always use contextBridge, never `nodeIntegration: true`:**

```typescript
// preload.ts
contextBridge.exposeInMainWorld('api', {
  readFile: (path: string) => ipcRenderer.invoke('read-file', path),
  saveFile: (path: string, content: string) => ipcRenderer.invoke('save-file', path, content),
});
```

### Tauri

```
desktop/tauri/
├── src-tauri/        ← Rust backend
│   ├── src/main.rs
│   └── tauri.conf.json
└── src/              ← Frontend (React/Vue)
```

**Tauri commands:**

```rust
#[tauri::command]
async fn read_file(path: String) -> Result<String, String> {
    fs::read_to_string(path).map_err(|e| e.to_string())
}
```

## IPC patterns

- Keep IPC surface minimal – expose only what the renderer needs
- Type all IPC messages with shared interfaces
- Handle errors in main process and return structured responses

```typescript
// Typed IPC in Electron
interface IFileApi {
  readFile(path: string): Promise<string>;
  saveFile(path: string, content: string): Promise<void>;
}
```

## Performance considerations

- Heavy computation → move to main process or Web Worker
- File I/O → always async
- Large data → stream instead of loading entirely into memory
- Window startup → lazy-load renderer content

## Testing desktop apps

```bash
npm run test -w desktop/<app>
# Integration tests: use @playwright/test with Electron driver
```
