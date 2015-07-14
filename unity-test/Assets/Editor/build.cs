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
		
		PlayerSettings.Android.bundleVersionCode = 1;
		
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
}