using Hexa.NET.GLFW;
using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Backends.GLFW;
using Hexa.NET.ImGui.Backends.Vulkan;
using System.Runtime.CompilerServices;

using GLFWmonitorPtr = Hexa.NET.GLFW.GLFWmonitorPtr;
using GLFWwindowPtr = Hexa.NET.GLFW.GLFWwindowPtr;

unsafe
{
  // Optional: set an error callback if available in your environment.
  // If NativeCallback is unavailable, skip setting the error callback.
  //GLFW.SetErrorCallback(error);

  if (GLFW.Init() == 0)
  {
    Console.WriteLine("Failed to initialize GLFW");
    return;
  }

  GLFW.WindowHint(GLFW.GLFW_CLIENT_API, GLFW.GLFW_NO_API); // Vulkan: no OpenGL context
  var mon = GLFW.GetPrimaryMonitor();
  float mainScale = ImGuiImplGLFW.GetContentScaleForMonitor(Unsafe.BitCast<GLFWmonitorPtr, Hexa.NET.ImGui.Backends.GLFW.GLFWmonitorPtr>(mon));
  GLFWwindowPtr window = GLFW.CreateWindow((int)(1280 * mainScale), (int)(800 * mainScale), "Hexa.NET ImGui GLFW+Vulkan", null, null);
  if (window.IsNull)
  {
    Console.WriteLine("Failed to create GLFW window.");
    GLFW.Terminate();
    return;
  }

  var guiContext = ImGui.CreateContext();
  ImGui.SetCurrentContext(guiContext);

  var io = ImGui.GetIO();
  io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
  io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
  ImGui.StyleColorsDark();

  var style = ImGui.GetStyle();
  style.ScaleAllSizes(mainScale);
  style.FontScaleDpi = mainScale;
  io.ConfigDpiScaleFonts = true;

  ImGuiImplGLFW.SetCurrentContext(guiContext);
  if (!ImGuiImplGLFW.InitForVulkan(Unsafe.BitCast<GLFWwindowPtr, Hexa.NET.ImGui.Backends.GLFW.GLFWwindowPtr>(window), true))
  {
    Console.WriteLine("Failed to initialize ImGui GLFW backend for Vulkan.");
    GLFW.DestroyWindow(window);
    GLFW.Terminate();
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

  while (GLFW.WindowShouldClose(window) == 0)
  {
    GLFW.PollEvents();

    if (GLFW.GetWindowAttrib(window, GLFW.GLFW_ICONIFIED) != 0)
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

  GLFW.DestroyWindow(window);
  GLFW.Terminate();
}