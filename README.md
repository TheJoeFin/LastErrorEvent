# Last Error Event

`lee` is a Windows-only .NET global tool that finds the newest **Application Error** event in the Windows **Application** log, formats it for LLM debugging, and copies it to your clipboard.

## Usage

```powershell
lee
```

By default, `lee` only returns a matching event if it happened within the last **1 hour**. If nothing recent is found, it tells you and offers to open Event Viewer.

## Options

```text
lee [--max-age-hours <hours>] [--open-event-viewer] [--no-prompt]
```

- `--max-age-hours <hours>`: override the default 1-hour recency window.
- `--open-event-viewer`: open Event Viewer automatically when no recent event is found.
- `--no-prompt`: suppress the interactive Event Viewer prompt.

## Install as a global tool from this repo

```powershell
dotnet pack .\src\LastErrorEvent\LastErrorEvent.csproj -c Release
dotnet tool install --global --add-source .\src\LastErrorEvent\bin\Release LastErrorEvent.Tool
```

If you rebuild locally and want to reinstall:

```powershell
dotnet tool update --global --add-source .\src\LastErrorEvent\bin\Release LastErrorEvent.Tool
```

## Local development

```powershell
dotnet build .\LastErrorEvent.slnx
dotnet test .\LastErrorEvent.slnx
dotnet run --project .\src\LastErrorEvent -- --help
```
