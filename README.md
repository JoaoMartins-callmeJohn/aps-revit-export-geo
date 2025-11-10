# Revit Geo Export Plugin

A .NET 8 class library template for developing Revit plugins with debugging support in Visual Studio Code with Cursor. This plugin exports geodata from Revit models and includes automated deployment for debugging.

## Prerequisites

* .NET 8.0 SDK or later
* Revit 2025
* Visual Studio Code with C# extension or Cursor IDE
* Windows PowerShell for build scripts

## Quick Setup

### 1. Configure Revit Paths

You need to **update the Revit paths** to match your installation:

#### In `RevitPlugin.csproj`:

```xml
<!-- Edit this path to match your Revit executable path -->
<RevitPath>C:\Program Files\Autodesk\Revit 2025</RevitPath>
<!-- Edit this path to match your Revit Addins path -->
<RevitAddinsPath>C:\ProgramData\Autodesk\Revit\Addins\2025</RevitAddinsPath>
```

Common Revit installation paths:
* Revit 2025: `C:\Program Files\Autodesk\Revit 2025`

### 2. Build the Project

Open a terminal in the project directory and run:

```bash
dotnet build -c Debug -p:Platform=x64
```

The DLL will be created in `bin\Debug\RevitPlugin.dll`

> **Note:** The build process automatically updates the addin file paths and copies the debug addin to the Revit addins folder.

### 3. Debug in Cursor/VS Code

1. Set breakpoints in `Commands.cs` (e.g., in your command methods)
2. Build the project using the command above
3. Start Revit manually
4. In Cursor/VS Code, press `F5` to attach debugger
5. Select the `Revit.exe` process when prompted
6. Test your commands in Revit

Your breakpoints will be hit!

## Build Configuration

The project includes several automated features for debugging:

### Pre-Build Events
- Runs `UpdateAddinPath.ps1` to update the assembly path in the addin file

### Post-Build Events
- Conditionally copies `GeoExportDebug.addin` to the Revit addins folder (only if it doesn't exist)

## Project Structure

```
aps-revit-export-geo/
├── RevitPlugin.csproj        # Project configuration
├── Commands.cs               # Revit command implementations
├── GeoExportDebug.addin      # Debug addin manifest
├── UpdateAddinPath.ps1       # Script to update addin paths
├── package-bundle.ps1        # Bundle packaging script
├── Properties/
├── .vscode/
│   ├── launch.json          # Debug configuration
│   └── tasks.json           # Build tasks
├── GeoExport.bundle/        # Deployment bundle
│   ├── PackageContents.xml  # Bundle manifest
│   └── Contents/
│       └── GeoExport.addin  # Release addin manifest
└── README.md                # This file
```

## Debugging Configuration

The template includes debug configurations in `.vscode/launch.json` for attaching to Revit.

### VS Code Tasks

Available tasks in `.vscode/tasks.json`:
- `build` - Default build task
- `clean` - Clean build outputs
- `rebuild` - Clean and rebuild

## Addin Management

### Debug Mode
During debug builds:
1. `UpdateAddinPath.ps1` updates the assembly path in `GeoExportDebug.addin`
2. The addin is copied to Revit's addins folder if not already present
3. No automatic cleanup is performed to allow for persistent debugging

### Release Mode
For release deployment, use the `package-bundle.ps1` script to create a proper bundle structure.

## Customization

### Adding New Commands

Add new command methods in `Commands.cs`:

```csharp
[Transaction(TransactionMode.Manual)]
public class MyCommand : IExternalCommand
{
    public Result Execute(
        ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
    {
        // Your command implementation
        return Result.Succeeded;
    }
}
```

### Changing Target Revit Version

1. Update `<RevitPath>` and `<RevitAddinsPath>` in the project file
2. Update the version number in addin files
3. Ensure the Revit API references match your target version

## Troubleshooting

### "Could not load file or assembly" error
* Ensure Revit paths are correct in the project file
* Verify you're building for x64 platform
* Check that the Revit version matches the referenced DLLs

### Breakpoints not hitting
* Ensure you're running in Debug configuration
* Check that the loaded addin points to the correct DLL path
* Try rebuilding the project and restarting Revit

### Addin not appearing in Revit
* Verify the addin file is in the correct Revit addins folder
* Check the addin file syntax and assembly path
* Look for errors in Revit's journal file

### PowerShell script errors
* Ensure execution policy allows running scripts
* Check file paths in the scripts
* Run scripts manually to see detailed error messages

## Build Scripts

### UpdateAddinPath.ps1
Updates the assembly path in addin files to match the build output location.

### package-bundle.ps1
Creates a deployment bundle for distribution, including proper folder structure and manifest files.

## Additional Resources

* [Revit API Developer's Guide](https://help.autodesk.com/view/RVT/2025/ENU/?guid=Revit_API_Revit_API_Developers_Guide_html)
* [Revit API Reference](https://www.revitapidocs.com/)
* [Autodesk Platform Services](https://aps.autodesk.com/)

## License

This project is licensed under the terms specified in the LICENSE file.

## Author

Created for Autodesk Platform Services integration with Revit 2025.

