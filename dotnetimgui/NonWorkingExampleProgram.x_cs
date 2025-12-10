using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Backends.GLFW;
using Hexa.NET.ImGui.Backends.Vulkan;
using Silk.NET.Windowing;
using System.Runtime.CompilerServices;

using GLFWwindowPtr = Hexa.NET.ImGui.Backends.GLFW.GLFWwindowPtr;

unsafe
{
  var opts = WindowOptions.Default;
  opts.Title = "Hexa.NET ImGui GLFW+Vulkan";
  opts.Size = new Silk.NET.Maths.Vector2D<int>(1280, 800);
  opts.API = GraphicsAPI.None; // Vulkan surface, no GL context
  using var view = Window.Create(opts);
  view.Initialize();
  if (!view.Native.Glfw.HasValue)
  {
    Console.WriteLine("Silk.NET did not provide a GLFW window pointer.");
    return;
  }
  GLFWwindowPtr window = new GLFWwindowPtr((GLFWwindow*)view.Native.Glfw!.Value);

  var guiContext = ImGui.CreateContext();
  ImGui.SetCurrentContext(guiContext);

  var io = ImGui.GetIO();
  io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
  io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
  ImGui.StyleColorsDark();

  var style = ImGui.GetStyle();
  float mainScale = 1.0f;
  style.ScaleAllSizes(mainScale);
  style.FontScaleDpi = mainScale;
  io.ConfigDpiScaleFonts = true;

  ImGuiImplGLFW.SetCurrentContext(guiContext);
  if (!ImGuiImplGLFW.InitForVulkan(window, true))
  {
    Console.WriteLine("Failed to initialize ImGui GLFW backend for Vulkan.");
    return;
  }

  // Minimal Vulkan setup via backend helper (using default instance/device managed externally by the app).
  // Hexa.NET Vulkan backend expects an init info struct populated by the caller.
  ImGuiImplVulkanInitInfo initInfo = new()
  {
    MinImageCount = 2
  };
  ImGuiImplVulkan.SetCurrentContext(guiContext);
  ImGuiImplVulkan.Init(new ImGuiImplVulkanInitInfoPtr((ImGuiImplVulkanInitInfo*)Unsafe.AsPointer(ref initInfo)));

  bool showHello = true;

  while (!view.IsClosing)
  {
    view.DoEvents();

    if (!view.IsVisible)
    {
      ImGuiImplGLFW.Sleep(10);
      continue;
    }

    ImGuiImplVulkan.NewFrame();
    ImGuiImplGLFW.NewFrame();
    ImGui.NewFrame();

    if (showHello)
    {
      ImGui.Begin("Hello");
      ImGui.Text("Hello, world from Hexa.NET!"u8);
      ImGui.End();
    }

    ImGui.Render();

    // The actual Vulkan rendering of draw data is handled by your engine/app.
    // For a minimal demonstration, we call the default platform windows handling.
    if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
    {
      ImGui.UpdatePlatformWindows();
      ImGui.RenderPlatformWindowsDefault();
    }
  }

  ImGuiImplVulkan.Shutdown();
  ImGuiImplGLFW.Shutdown();
  ImGui.DestroyContext();

  view.Close();
}