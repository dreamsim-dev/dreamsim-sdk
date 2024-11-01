using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Dreamsim.CI
{
public class AutoKeystore : IPreprocessBuildWithReport
{
    public int callbackOrder => -999;

    private static string KeystorePasswordsDirectory
        => Application.dataPath + "/../KeystorePasswords/";

    private static string KeystorePasswordsFilename
        => "keystore-passwords.txt";

    private static string KeystorePasswordsFile
        => KeystorePasswordsDirectory + KeystorePasswordsFilename;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (Environment.GetCommandLineArgs().Contains("-keyaliaspass"))
        {
            DreamsimLogger.Log("Set keystore settings from command line arguments");
            return;
        }

        if (report.summary.platform != BuildTarget.Android) return;

        if (!File.Exists(KeystorePasswordsFile))
        {
            MakeTemplate();
            OpenKeystoreFile();
            return;
        }

        var keystorePasswords =
            JsonUtility.FromJson<KeystorePasswords>(File.ReadAllText(KeystorePasswordsFile));

        PlayerSettings.Android.keystorePass =
            keystorePasswords.keystorePass;
        PlayerSettings.Android.keyaliasName =
            keystorePasswords.keyAliasName;
        PlayerSettings.Android.keyaliasPass =
            keystorePasswords.keyAliasPass;
    }

    private static void MakeTemplate()
    {
        var keystorePasswords = new KeystorePasswords
        {
            keystorePass = PlayerSettings.Android.keystorePass,
            keyAliasName = PlayerSettings.Android.keyaliasName,
            keyAliasPass = PlayerSettings.Android.keyaliasPass
        };

        Directory.CreateDirectory(KeystorePasswordsDirectory);
        File.WriteAllText(KeystorePasswordsFile,
            JsonUtility.ToJson(keystorePasswords, true));
        File.WriteAllText(KeystorePasswordsDirectory + ".gitignore",
            KeystorePasswordsFilename);
    }

    private static void OpenKeystoreFile() { Process.Start(KeystorePasswordsFile); }
}

public class KeystorePasswords
{
    public string keystorePass = string.Empty;
    public string keyAliasName = string.Empty;
    public string keyAliasPass = string.Empty;
}
}