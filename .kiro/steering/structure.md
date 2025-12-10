# Project Structure

## Root Directory
```
├── dotnetimgui/           # Main application project
├── dotnetimgui.slnx       # Solution file
├── README.md              # Project documentation
├── LICENSE.txt            # License information
└── .kiro/                 # Kiro configuration and steering
```

## Application Project (`dotnetimgui/`)
```
dotnetimgui/
├── Program.cs             # Main application entry point
├── dotnetimgui.csproj     # Project file with dependencies
├── bin/                   # Build output (generated)
└── obj/                   # Build intermediates (generated)
```

## Code Organization

### Main Application (`Program.cs`)
- **Single file application**: All code in Program.cs for simplicity
- **Top-level statements**: Uses modern C# top-level program structure
- **Unsafe context**: Entire main block runs in unsafe context for native interop

### Key Code Sections
1. **Window Setup**: Silk.NET windowing configuration
2. **ImGui Context**: ImGui context creation and configuration
3. **Backend Initialization**: GLFW and Vulkan backend setup
4. **Render Loop**: Main application loop with ImGui rendering
5. **Cleanup**: Proper resource disposal

## Build Artifacts
- `bin/`: Contains compiled executables and runtime files
- `obj/`: Contains intermediate build files, cache, and metadata
- Both directories are auto-generated and should not be committed to version control

## Conventions
- **Minimal structure**: Keep the project as simple as possible
- **Single executable**: Console application with GUI window
- **Native interop**: Use unsafe pointers where required for ImGui integration
- **Resource management**: Proper initialization and cleanup of graphics resources