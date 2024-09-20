using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;

#if UNITY_IOS
using System.IO;
using UnityEditor.iOS.Xcode;
#endif

namespace Dreamsim.Publishing.Editor
{
public class BuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    
    public void OnPreprocessBuild(BuildReport report)
    {
        // Intentionally empty
    }

    [PostProcessBuild(96)]
    private static void First(BuildTarget target, string path)
    {
        #if UNITY_IOS
        var plistPath = path + $"{Path.DirectorySeparatorChar}Info.plist";
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        var plistRoot = plist.root;

        plistRoot.SetString("NSUserTrackingUsageDescription",
            "Your data will be used to provide you a better and personalized ad experience.");
        
        var skAdNetworkItems = plist.root["SKAdNetworkItems"].AsArray();
        var skAdNetworkItemsDict = skAdNetworkItems.AddDict().AsDict();
        skAdNetworkItemsDict.values.Add("SKAdNetworkIdentifier", new PlistElementString("su67r6k2v3.skadnetwork"));

        plist.root.values.Add("NSAdvertisingAttributionReportEndpoint",
            new PlistElementString("https://postbacks-is.com"));

        plist.root.values.Add("NSAppTransportSecurity", new PlistElementDict());

        plist.root.SetString("NSUserTrackingUsageDescription",
            "This identifier will be used to deliver personalized ads to you.");

        var nsAppTransportSecurity = plist.root["NSAppTransportSecurity"].AsDict();
        nsAppTransportSecurity.values.Remove("NSAllowsArbitraryLoadsInWebContent");
        nsAppTransportSecurity.values.Add("NSAllowsArbitraryLoads", new PlistElementBoolean(true));

        plist.WriteToFile(plistPath);
        #endif
    }

    [PostProcessBuild(97)]
    public static void Second(BuildTarget target, string path)
    {
        #if UNITY_IOS
        const string entitlementsFileName = "Entitlements.entitlements";
        
        var projectPath = PBXProject.GetPBXProjectPath(path);
        var pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);

        var mainTargetGuid = pbxProject.GetUnityMainTargetGuid();
        pbxProject.AddFrameworkToProject(mainTargetGuid, "AdSupport.framework", true);
        pbxProject.AddFrameworkToProject(mainTargetGuid, "AppTrackingTransparency.framework", true);

        var frameworkTargetGuid = pbxProject.GetUnityFrameworkTargetGuid();
        pbxProject.SetBuildProperty(frameworkTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");

        var manager = new ProjectCapabilityManager(projectPath, entitlementsFileName, null, mainTargetGuid);
        manager.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications);
        manager.AddPushNotifications(false);
        manager.WriteToFile();

        pbxProject.WriteToFile(projectPath);
        #endif
    }
}
}