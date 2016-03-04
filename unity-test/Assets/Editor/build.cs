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

    [UnityEditor.MenuItem("Tools/Disable")]
    static void disableUWP()
    {
        PluginImporter pi = (PluginImporter)PluginImporter.GetAtPath("Assets/Plugins/Metro/UWP/VungleSDK.winmd");
        pi.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
        pi.SaveAndReimport();
    }

    static void BuildUWP()
    {
		PlayerSettings.bundleIdentifier = "com.vungle.unity5test";
		PlayerSettings.companyName = "Vungle";
		PlayerSettings.productName = "VungleUnity5Test_win10";
		PlayerSettings.bundleVersion = CommandLineReader.GetCustomArgument("ver");
        	PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.InternetClient, true);
        	PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.Location, true);
        	PlayerSettings.WSA.packageVersion = new Version(CommandLineReader.GetCustomArgument("ver"));

        	EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WSAPlayer);
        	#if UNITY_WSA_10_0 || UNITY_WINRT_8_1 || UNITY_METRO
		EditorUserBuildSettings.wsaSDK = WSASDK.UWP;
		#endif
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

    static void BuildWin81()
	{
        makeMetro();

        PlayerSettings.bundleIdentifier = "com.vungle.unity5test";
		PlayerSettings.companyName = "Vungle";
		PlayerSettings.productName = "VungleUnity5Test_win81";
		PlayerSettings.bundleVersion = CommandLineReader.GetCustomArgument("ver");
        PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.InternetClient, true);
        PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.Location, true);
        PlayerSettings.WSA.packageVersion = new Version(CommandLineReader.GetCustomArgument("ver"));

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WSAPlayer);
		EditorUserBuildSettings.wsaSDK = WSASDK.SDK81;
		var original = EditorBuildSettings.scenes;
		var newSettings = new EditorBuildSettingsScene[original.Length + 1];
		System.Array.Copy(original, newSettings, original.Length);
		var sceneToAdd = new EditorBuildSettingsScene("Assets/Plugins/Vungle/test/test.unity", true);
		newSettings[newSettings.Length - 1] = sceneToAdd;
		EditorBuildSettings.scenes = newSettings;
		string[] scenes = new string[1];
		scenes [0] = sceneToAdd.path;

	disableUWP();

		BuildPipeline.BuildPlayer(scenes, "Builds/WSA", BuildTarget.WSAPlayer, BuildOptions.None);

	}
    static void BuildWP81()
	{
        makeMetro();

        PlayerSettings.bundleIdentifier = "com.vungle.unity5test";
		PlayerSettings.companyName = "Vungle";
		PlayerSettings.productName = "VungleUnity5Test_wp81";
		PlayerSettings.bundleVersion = CommandLineReader.GetCustomArgument("ver");
        PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.InternetClient, true);
        PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.Location, true);
        PlayerSettings.WSA.packageVersion = new Version(CommandLineReader.GetCustomArgument("ver"));

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WSAPlayer);
		EditorUserBuildSettings.wsaSDK = WSASDK.PhoneSDK81;
		var original = EditorBuildSettings.scenes;
		var newSettings = new EditorBuildSettingsScene[original.Length + 1];
		System.Array.Copy(original, newSettings, original.Length);
		var sceneToAdd = new EditorBuildSettingsScene("Assets/Plugins/Vungle/test/test.unity", true);
		newSettings[newSettings.Length - 1] = sceneToAdd;
		EditorBuildSettings.scenes = newSettings;
		string[] scenes = new string[1];
		scenes [0] = sceneToAdd.path;

	disableUWP();

		BuildPipeline.BuildPlayer(scenes, "Builds/WSA", BuildTarget.WSAPlayer, BuildOptions.None);

	}
}
