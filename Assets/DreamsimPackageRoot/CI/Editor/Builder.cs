// ReSharper disable RedundantUsingDirective

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;

#if UNITY_IPHONE || UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace Dreamsim.CI
{
public class Builder
{
    #if UNITY_IPHONE || UNITY_IOS
    private const string ExportOptionsContents =
        "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"
        + "<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">"
        + "\n<plist version=\"1.0\">\n"
        + "    <dict>\n"
        + "        <key>method</key>\n"
        + "        <string>app-store</string>\n"
        + "        <key>destination</key>\n"
        + "        <string>upload</string>\n"
        + "    </dict>\n</plist>";

    /// <summary>
    /// Necessary action to export iOS app using CLI
    /// </summary>
    [PostProcessBuild]
    public static void UpdateUploadToken(BuildTarget target, string path)
    {
        var pbxFilename =
            path + "/Unity-iPhone.xcodeproj/project.pbxproj";
        var project = new PBXProject();
        project.ReadFromFile(pbxFilename);

        var targetGuid =
            project.GetUnityMainTargetGuid(); //Unity 2019.3 or newer only
        var token =
            project.GetBuildPropertyForAnyConfig(targetGuid, "USYM_UPLOAD_AUTH_TOKEN");

        if (string.IsNullOrEmpty(token)) token = "FakeToken";

        project.SetBuildProperty(targetGuid, "USYM_UPLOAD_AUTH_TOKEN", token);
        project.WriteToFile(pbxFilename);
    }

    /// <summary>
    /// Necessary action to export iOS app using CLI
    /// </summary>
    [PostProcessBuild]
    public static void CopyExportOptionsPlist(BuildTarget target,
        string path)
    {
        const string deployPlistName = "exportOptions.plist";
        var deployPlistDestination = Path.Combine(path, deployPlistName);
        File.WriteAllText(deployPlistDestination, ExportOptionsContents);
    }

    /// <summary>
    /// Avoid missing compliance questions
    /// </summary>
    [PostProcessBuild]
    public static void SetNonExemptEncryptionKey(BuildTarget target, string path)
    {
        var plistPath = Path.Combine(path, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);
        plist.root.SetBoolean("ITSAppUsesNonExemptEncryption", false);
        File.WriteAllText(plistPath, plist.WriteToString());
    }

    #endif


    /// <summary>
    /// Key CI CLI method to start building.
    /// Use with "-build-location /path/to" and "-build-target platform" keys.
    /// </summary>
    public static void Build()
    {
        var scenes = EditorBuildSettings.scenes.Select(x => x.path)
            .ToArray();
        var buildLocation = GetArgumentValue("-build-location");
        var target = GetTargetFromCliArg();
        var options = BuildOptions.None;
        var buildNumber = GetArgumentValue("-build-number");

        var targetFile = Resources.Load<TextAsset>("[Dreamsim] BuildNumber");
        File.WriteAllText(AssetDatabase.GetAssetPath(targetFile), buildNumber);
        EditorUtility.SetDirty(targetFile);

        PlayerSettings.iOS.buildNumber = buildNumber;
        PlayerSettings.Android.bundleVersionCode = int.Parse(buildNumber);
        EditorUserBuildSettings.buildAppBundle = true;

        PlayerSettings.SplashScreen.showUnityLogo = false;

        PlayerSettings.Android.keystorePass = GetArgumentValue("-keystorepass");
        PlayerSettings.Android.keyaliasName = GetArgumentValue("-keyaliasname");
        PlayerSettings.Android.keyaliasPass = GetArgumentValue("-keyaliaspass");

        BuildPipeline.BuildPlayer(scenes, buildLocation, target, options);
    }

    private static BuildTarget GetTargetFromCliArg()
    {
        var value = GetArgumentValue("-buildTarget");

        switch (value)
        {
        case "iOS": return BuildTarget.iOS;
        case "Android": return BuildTarget.Android;
        default: throw new Exception($"You must provide 'buildTarget' key with 'iOS' or 'Android' value.");
        }
    }

    private static string GetArgumentValue(string key)
    {
        var args = Environment.GetCommandLineArgs().ToList();
        var argIndex = args.FindIndex(x => x == key);
        var value = args[argIndex + 1];

        return value;
    }
}
}