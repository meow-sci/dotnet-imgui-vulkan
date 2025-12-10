# Technology Stack

## Framework & Runtime
- **.NET 10**: Target framework for the application
- **C# with unsafe blocks**: Required for ImGui native interop

## Core Dependencies
- **Hexa.NET.ImGui** (v2.2.9): C# bindings for ImGui
- **Hexa.NET.ImGui.Backends** (v1.0.18): ImGui backend implementations
- **Hexa.NET.ImGui.Backends.GLFW** (v1.0.18): GLFW windowing backend
- **Hexa.NET.Vulkan** (v1.1.0): Vulkan API bindings
- **Silk.NET.Windowing** (v2.22.0): Cross-platform windowing
- **Silk.NET.Vulkan** (v2.22.0): Additional Vulkan support

## Graphics Stack
- **Vulkan**: Primary rendering backend (required)
- **GLFW**: Windowing system backend
- **ImGui**: Immediate mode GUI framework

## Project Configuration
- **Unsafe blocks enabled**: Required for native interop
- **Implicit usings enabled**: Modern C# feature
- **Nullable reference types enabled**: Enhanced type safety

## Common Commands

### Build
```bash
dotnet build dotnetimgui/dotnetimgui.csproj
```

### Run
```bash
dotnet run --project dotnetimgui/dotnetimgui.csproj
```

### Clean
```bash
dotnet clean dotnetimgui/dotnetimgui.csproj
```

### Restore Dependencies
```bash
dotnet restore dotnetimgui/dotnetimgui.csproj
```