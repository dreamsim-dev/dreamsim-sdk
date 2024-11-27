using IngameDebugConsole;

namespace Dreamsim.Publishing
{ 
// ReSharper disable once UnusedType.Global
public static class ConsoleMethods
{
    [ConsoleMethod("debugads", "Validate advertisement integration")]
    // ReSharper disable once UnusedMember.Global
    public static void DebugAds() { DreamsimPublishing.Advertisement.ValidateIntegration(); }
}
}