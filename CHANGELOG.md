# Changelog

All notable changes to the CoverTree Visual Studio extension are documented in this file.

## 1.0.0

Initial release — a port of [CoverTree for VS Code](https://github.com/WalSplitter/covertree-vscode) to Visual Studio 2022.

- **CoverTree tool window** — tree view of coverage per file/folder, mirroring the structure shown in Solution Explorer, with color-coded pass/warning indicators and a configurable threshold.
- **Solution Explorer integration** — an expandable coverage row under each covered source file with a Lines/Functions/Branches breakdown.
- **Editor gutter markers** — green/red/yellow line-level coverage indicators in the margin, based on `coverage-final.json`.
- **Navigate uncovered lines** — `Alt+Shift+N` / `Alt+Shift+P` jump to the next/previous uncovered line in the active file.
- **Status bar** — overall coverage percentage across all discovered files.
- **Automatic coverage discovery** — recursively scans the whole solution/workspace for `coverage-summary.json` / `coverage-final.json`, regardless of which sub-project folder they live in, skipping `node_modules`, `.git`, `bin`, `obj`, and similar directories. No manual path configuration needed for the common case.
- Supports both a full Visual Studio Solution (`.sln`) and **File > Open Folder** workflows.
- Options page under **Tools > Options > CoverTree** for threshold, file names, and gutter marker toggle.
