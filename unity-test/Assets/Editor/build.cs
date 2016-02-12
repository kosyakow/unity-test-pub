using System;
using UnityEditor;
using UnityEngine;

class BuildGenerator
{
	static void BuildAndroid()
	{
		PlayerSettings.bundleIdentifier = "com.vungle.unitytest";
		PlayerSettings.companyName = "Vungle";
		PlayerSettings.productName = "VungleUnityTest";
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
		PlayerSettings.bundleIdentifier = "com.vungle.unitytest";
		PlayerSettings.companyName = "Vungle";
		PlayerSettings.productName = "VungleUnityTest";
		PlayerSettings.bundleVersion = CommandLineReader.GetCustomArgument("ver");

		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iPhone);

		var original = EditorBuildSettings.scenes;
		var newSettings = new EditorBuildSettingsScene[original.Length + 1];
		System.Array.Copy(original, newSettings, original.Length);
		var sceneToAdd = new EditorBuildSettingsScene("Assets/Plugins/Vungle/test/test.unity", true);
		newSettings[newSettings.Length - 1] = sceneToAdd;
		EditorBuildSettings.scenes = newSettings;
	}

	[UnityEditor.MenuItem("Tools/MakeMetro")]
	private static void makeMetro()
	{
		var unprocessedPlugins = PlayerSettings.Metro.unprocessedPlugins;
		bool find = false;
		foreach(var plugin in unprocessedPlugins)
			if ("VungleSDKProxy.dll".Equals(plugin)) {
			find = true;
			break;
		}
		if (!find) {
			var newUnprocessedPlugins = new String[unprocessedPlugins.Length + 1];
			System.Array.Copy(unprocessedPlugins, newUnprocessedPlugins, unprocessedPlugins.Length);
			newUnprocessedPlugins[newUnprocessedPlugins.Length - 1] = "VungleSDKProxy.dll";
			PlayerSettings.Metro.unprocessedPlugins = newUnprocessedPlugins;
		}
	}

	/*
	[UnityEditor.MenuItem("Tools/Disable")]
	static void disableUWP()
	{
        	PluginImporter pi = (PluginImporter)PluginImporter.GetAtPath("Assets/Plugins/Metro/UWP/VungleSDK.winmd");
        	pi.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);
        	pi.SaveAndReimport();
    	}
    	*/
    	/*
	static void BuildUWP()
	{
		PlayerSettings.bundleIdentifier = "com.vungle.unitytest";
		PlayerSettings.companyName = "Vungle";
		PlayerSettings.productName = "VungleUnityTest_win10";
		PlayerSettings.bundleVersion = CommandLineReader.GetCustomArgument("ver");
        	PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.InternetClient, true);
        	PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.Location, true);
        	PlayerSettings.WSA.packageVersion = new Version(CommandLineReader.GetCustomArgument("ver"));

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
	*/

    	static void BuildWin81()
	{
        	makeMetro();

        	PlayerSettings.bundleIdentifier = "com.vungle.unitytest";
		PlayerSettings.companyName = "Vungle";
		PlayerSettings.productName = "VungleUnityTest_win81";
		PlayerSettings.bundleVersion = CommandLineReader.GetCustomArgument("ver");

		PlayerSettings.Metro.SetCapability(PlayerSettings.MetroCapability.InternetClient, true);
		PlayerSettings.Metro.SetCapability(PlayerSettings.MetroCapability.Location, true);
		PlayerSettings.Metro.packageVersion = new Version(CommandLineReader.GetCustomArgument("ver"));

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.MetroPlayer);
		EditorUserBuildSettings.metroSDK = MetroSDK.SDK81;
		var original = EditorBuildSettings.scenes;
		var newSettings = new EditorBuildSettingsScene[original.Length + 1];
		System.Array.Copy(original, newSettings, original.Length);
		var sceneToAdd = new EditorBuildSettingsScene("Assets/Plugins/Vungle/test/test.unity", true);
		newSettings[newSettings.Length - 1] = sceneToAdd;
		EditorBuildSettings.scenes = newSettings;
		string[] scenes = new string[1];
		scenes [0] = sceneToAdd.path;

		//disableUWP();

		BuildPipeline.BuildPlayer(scenes, "Builds/WSA", BuildTarget.MetroPlayer, BuildOptions.None);

	}
	static void BuildWP81()
	{
        	makeMetro();

        	PlayerSettings.bundleIdentifier = "com.vungle.unitytest";
		PlayerSettings.companyName = "Vungle";
		PlayerSettings.productName = "VungleUnityTest_wp81";
		PlayerSettings.bundleVersion = CommandLineReader.GetCustomArgument("ver");
		PlayerSettings.Metro.SetCapability(PlayerSettings.MetroCapability.InternetClient, true);
		PlayerSettings.Metro.SetCapability(PlayerSettings.MetroCapability.Location, true);
		PlayerSettings.Metro.packageVersion = new Version(CommandLineReader.GetCustomArgument("ver"));

		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.MetroPlayer);
		EditorUserBuildSettings.metroSDK = MetroSDK.PhoneSDK81;
		var original = EditorBuildSettings.scenes;
		var newSettings = new EditorBuildSettingsScene[original.Length + 1];
		System.Array.Copy(original, newSettings, original.Length);
		var sceneToAdd = new EditorBuildSettingsScene("Assets/Plugins/Vungle/test/test.unity", true);
		newSettings[newSettings.Length - 1] = sceneToAdd;
		EditorBuildSettings.scenes = newSettings;
		string[] scenes = new string[1];
		scenes [0] = sceneToAdd.path;

		//disableUWP();

		BuildPipeline.BuildPlayer(scenes, "Builds/WSA", BuildTarget.MetroPlayer, BuildOptions.None);

	}
}