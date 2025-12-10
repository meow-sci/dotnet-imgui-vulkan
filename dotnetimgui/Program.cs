using Silk.NET.Windowing;

unsafe
{
    // Initialize window configuration
    var windowConfig = new WindowConfig("Minimal ImGui Application", 800, 600, true, true);

    // Create window options
    var windowOptions = WindowOptions.Default;
    windowOptions.Title = windowConfig.Title ?? "Minimal ImGui Application";
    windowOptions.Size = new Silk.NET.Maths.Vector2D<int>(windowConfig.Width, windowConfig.Height);
    windowOptions.WindowBorder = windowConfig.Resizable ? WindowBorder.Resizable : WindowBorder.Fixed;
    windowOptions.VSync = windowConfig.VSync;
    // Use OpenGL for now - will be replaced with Vulkan in later tasks
    windowOptions.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.ForwardCompatible, new APIVersion(3, 3));

    IWindow? window = null;

    try
    {
        Console.WriteLine("Creating window...");
        
        // Create GLFW window using Silk.NET.Windowing
        window = Window.Create(windowOptions);
        
        if (window == null)
        {
            throw new InvalidOperationException("Failed to create window: Window.Create returned null");
        }

        Console.WriteLine($"Successfully created window: {windowConfig.Title} ({windowConfig.Width}x{windowConfig.Height})");
        Console.WriteLine($"Window properties - Resizable: {windowConfig.Resizable}, VSync: {windowConfig.VSync}");

        // Set up window event handlers
        window.Load += () =>
        {
            Console.WriteLine("Window loaded successfully");
        };

        window.Closing += () =>
        {
            Console.WriteLine("Window closing...");
        };

        window.Resize += (size) =>
        {
            Console.WriteLine($"Window resized to: {size.X}x{size.Y}");
        };

        Console.WriteLine("Initializing window...");
        // Initialize and run the window
        window.Initialize();
        
        Console.WriteLine("Starting window run loop...");
        // For now, just run a simple message loop to verify window creation
        // This will be replaced with the full render loop in later tasks
        window.Run();
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error creating or running window: {ex.Message}");
        Console.Error.WriteLine($"Stack trace: {ex.StackTrace}");
        
        // Provide helpful error context
        Console.Error.WriteLine("\nTroubleshooting information:");
        Console.Error.WriteLine("- Ensure graphics drivers are up to date");
        Console.Error.WriteLine("- Verify system supports the requested window configuration");
        Console.Error.WriteLine($"- Requested configuration: {windowConfig.Width}x{windowConfig.Height}, Resizable: {windowConfig.Resizable}");
        
        Environment.Exit(1);
    }
    finally
    {
        // Cleanup window resources
        window?.Dispose();
        Console.WriteLine("Window resources cleaned up");
    }
}

// Window configuration structure
struct WindowConfig
{
    public string Title;
    public int Width;
    public int Height;
    public bool Resizable;
    public bool VSync;

    public WindowConfig(string title = "Minimal ImGui Application", int width = 800, int height = 600, bool resizable = true, bool vSync = true)
    {
        Title = title ?? "Minimal ImGui Application";
        Width = width;
        Height = height;
        Resizable = resizable;
        VSync = vSync;
    }
}