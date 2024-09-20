using IngameDebugConsole;

namespace Dreamsim
{ 
// ReSharper disable once UnusedType.Global
public static class ConsoleMethods
{
    [ConsoleMethod("debugads", "Validate advertisement integration")]
    // ReSharper disable once UnusedMember.Global
    public static void DebugAds() { DreamsimApp.Advertisement.ValidateIntegration(); }
}
}