using Silk.NET.Windowing;
using Hexa.NET.ImGui;

unsafe
{
    // ImGui context and backend initialization
    // This implements ImGui context creation and backend configuration as specified in task 3.1
    static unsafe void InitializeImGuiContext(IWindow window)
    {
        try
        {
            Console.WriteLine("Initializing ImGui context...");
            
            // Create ImGui context
            var context = ImGui.CreateContext();
            if (context.IsNull)
            {
                throw new InvalidOperationException("Failed to create ImGui context");
            }
            ImGui.SetCurrentContext(context);
            Console.WriteLine("ImGui context created successfully");
            
            // Configure ImGui IO settings
            var io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;  // Enable keyboard navigation
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;      // Enable docking
            Console.WriteLine("ImGui IO configuration applied");
            
            // Set up ImGui style - using a clean, modern dark theme
            ImGui.StyleColorsDark();
            var style = ImGui.GetStyle();
            style.WindowRounding = 5.0f;
            style.FrameRounding = 3.0f;
            style.ScrollbarRounding = 3.0f;
            style.GrabRounding = 3.0f;
            style.TabRounding = 3.0f;
            Console.WriteLine("ImGui style configured with dark theme");
            
            // Note: Backend initialization (GLFW and Vulkan) will be handled in subsequent tasks
            // For now, we have successfully set up the ImGui context with proper configuration
            Console.WriteLine("ImGui context ready for backend integration");
            
            Console.WriteLine("ImGui context initialized successfully");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to initialize ImGui context: {ex.Message}");
            
            // Provide helpful troubleshooting information
            Console.Error.WriteLine("\nImGui initialization troubleshooting:");
            Console.Error.WriteLine("- Ensure Hexa.NET.ImGui packages are properly installed");
            Console.Error.WriteLine("- Verify ImGui context creation succeeded");
            Console.Error.WriteLine("- Check that ImGui configuration flags are valid");
            
            throw;
        }
    }
    
    // Cleanup ImGui resources
    static unsafe void CleanupImGuiContext()
    {
        try
        {
            Console.WriteLine("Cleaning up ImGui resources...");
            
            // Destroy ImGui context
            ImGui.DestroyContext();
            
            Console.WriteLine("ImGui resources cleaned up successfully");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during ImGui cleanup: {ex.Message}");
        }
    }

    // Initialize window configuration
    var windowConfig = new WindowConfig("Minimal ImGui Application", 800, 600, true, true);

    // Create window options
    var windowOptions = WindowOptions.Default;
    windowOptions.Title = windowConfig.Title ?? "Minimal ImGui Application";
    windowOptions.Size = new Silk.NET.Maths.Vector2D<int>(windowConfig.Width, windowConfig.Height);
    windowOptions.WindowBorder = windowConfig.Resizable ? WindowBorder.Resizable : WindowBorder.Fixed;
    windowOptions.VSync = windowConfig.VSync;
    // Configure for Vulkan API
    windowOptions.API = GraphicsAPI.None; // No OpenGL context needed for Vulkan

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
            
            // Initialize ImGui context and backends after window is loaded
            InitializeImGuiContext(window);
        };

        window.Closing += () =>
        {
            Console.WriteLine("Window closing...");
            
            // Cleanup ImGui resources before window closes
            CleanupImGuiContext();
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
        // Window disposal will trigger the Closing event which handles ImGui cleanup
        window?.Dispose();
        Console.WriteLine("Window and ImGui resources cleaned up");
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

// Application state structure for ImGui integration
unsafe struct ApplicationState
{
    public IWindow Window;
    public IntPtr ImGuiContext;
    public bool IsRunning;
    public float DeltaTime;
}