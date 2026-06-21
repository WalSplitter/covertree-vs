# CoverTree for Visual Studio

C# VSIX extension porting the [covertree-vscode](https://github.com/WalSplitter/covertree-vscode) extension to Visual Studio 2022.

## Stack

- C# / .NET 4.7.2
- Visual Studio SDK 17.x (MEF + AsyncPackage)
- Newtonsoft.Json 13.x for parsing Istanbul coverage JSON

## Project structure

```
CoverTree.VS.sln
src/
  CoverTree.VS.csproj
  CoverTreePackage.cs          ← main VS package
  CoverTreePackage.vsct        ← command table (menus + keybindings)
  source.extension.vsixmanifest
  Coverage/
    CoverageModels.cs          ← data types
    CoverageParser.cs          ← parse coverage-summary.json
    DetailParser.cs            ← parse coverage-final.json (line map)
    CoverageService.cs         ← central service, file watcher
  Commands/
    RefreshCoverageCommand.cs
    NavigateUncoveredCommand.cs
  ToolWindow/
    CoverTreeToolWindow.cs     ← ToolWindowPane
    CoverTreeControl.xaml/.cs  ← WPF tree view
    CoverageViewModel.cs
    CoverageFileItem.cs
  Adornments/
    CoverageTag.cs             ← IGlyphTag
    CoverageGlyphTagger.cs
    CoverageGlyphTaggerProvider.cs
    CoverageGlyphFactory.cs    ← draws colored rectangle in gutter
    CoverageGlyphFactoryProvider.cs
  Options/
    CoverTreeSettings.cs       ← plain POCO
    CoverTreeOptionsPage.cs    ← DialogPage (Tools > Options > CoverTree)
```

## Key GUIDs

| Thing | GUID |
|-------|------|
| Package | `9F3A7B2C-D4E5-4F1B-A8C6-3E9F2B4D7A1C` |
| Command set | `4C8E1F3A-7D2B-4F6A-9B3E-5D1C8F2A4E7B` |
| Tool window | `7B1E4A8C-2D5F-4C9A-6E3B-8F4A1D7C2E5B` |

## Keyboard shortcuts

- `Alt+Shift+N` — next uncovered line
- `Alt+Shift+P` — previous uncovered line

## Build

Open `CoverTree.VS.sln` in Visual Studio 2022 with the **Visual Studio extension development** workload installed. Press F5 to launch the experimental instance.

## Coverage file locations (configurable via Tools > Options > CoverTree)

- Summary: `coverage/coverage-summary.json` (relative to solution root)
- Detail: `coverage/coverage-final.json`
- Threshold: 75%
