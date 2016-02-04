using System;
using UnityEditor;
using UnityEngine;

class BuildGenerator
{
	static void BuildAndroid()
	{
		PlayerSettings.bundleIdentifier = "com.vungle.unity5test";
		PlayerSettings.companyName = "Vungle";
		PlayerSettings.productName = "VungleUnity5Test";
		PlayerSettings.bundleVersion = CommandLineReader.GetCustomArgument("ver");
		
	
		PlayerSettings.Android.bundleVersionCode = 2;
		//PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.Auto;

		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
		var original = EditorBuildSettings.scenes;
		var newSettings = new EditorBuildSettingsScene[original.Length + 1];
		System.Array.Copy(original, newSettings, original.Length);
		var sceneToAdd = new EditorBuildSettingsScene("Assets/Plugins/Vungle/test/test.unity", true);
		newSettings[newSettings.Length - 1] = sceneToAdd;
		EditorBuildSettings.scenes = newSettings;
	}

	static void BuildiPhone()
	{
		PlayerSettings.bundleIdentifier = "com.vungle.unity5test";
		PlayerSettings.companyName = "Vungle";
		PlayerSettings.productName = "VungleUnity5Test";
		PlayerSettings.bundleVersion = CommandLineReader.GetCustomArgument("ver");
//		PlayerSettings.shortBundleVersion = CommandLineReader.GetCustomArgument("ver");
	#if UNITY5_SCRIPTING_IN_UNITY4
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iPhone);
	#else
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
	#endif
		var original = EditorBuildSettings.scenes;
		var newSettings = new EditorBuildSettingsScene[original.Length + 1];
		System.Array.Copy(original, newSettings, original.Length);
		var sceneToAdd = new EditorBuildSettingsScene("Assets/Plugins/Vungle/test/test.unity", true);
		newSettings[newSettings.Length - 1] = sceneToAdd;
		EditorBuildSettings.scenes = newSettings;
	}

    private static void makeMetro()
    {
        PluginImporter pi = (PluginImporter)PluginImporter.GetAtPath("Assets/Plugins/Metro/VungleSDKProxy.winmd");
        pi.SetPlatformData(BuildTarget.WSAPlayer, "PlaceholderPath", "Assets/Plugins/VungleSDKProxy.dll");
        pi.SaveAndReimport();
        pi = (PluginImporter)PluginImporter.GetAtPath("Assets/Plugins/Metro/VungleSDK.winmd");
        pi.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "SDK81");
        pi.SaveAndReimport();
    }

    static void BuildUWP()
	{
		PlayerSettings.bundleIdentifier = "com.vungle.unity5test";
		PlayerSettings.companyName = "Vungle";
		PlayerSettings.productName = "VungleUnity5Test";
		PlayerSettings.bundleVersion = CommandLineReader.GetCustomArgument("ver");
        PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.InternetClient, true);
        PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.Location, true);

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WSAPlayer);
		EditorUserBuildSettings.wsaSDK = WSASDK.UWP;
		var original = EditorBuildSettings.scenes;
		var newSettings = new EditorBuildSettingsScene[original.Length + 1];
		System.Array.Copy(original, newSettings, original.Length);
		var sceneToAdd = new EditorBuildSettingsScene("Assets/Plugins/Vungle/test/test.unity", true);
		newSettings[newSettings.Length - 1] = sceneToAdd;
		EditorBuildSettings.scenes = newSettings;
		string[] scenes = new string[1];
		scenes [0] = sceneToAdd.path;

        makeMetro();

		BuildPipeline.BuildPlayer(scenes, "Builds/WSA", BuildTarget.WSAPlayer, BuildOptions.None);

	}

}