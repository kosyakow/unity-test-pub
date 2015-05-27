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

		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
		var original = EditorBuildSettings.scenes; 
		
		var newSettings = new EditorBuildSettingsScene[original.Length + 1]; 
		
		System.Array.Copy(original, newSettings, original.Length); 
		
		var sceneToAdd = new EditorBuildSettingsScene("Assets/test.unity", true); 
		
		newSettings[newSettings.Length - 1] = sceneToAdd; 
		
		EditorBuildSettings.scenes = newSettings;
	}

	static void BuildiPhone()
	{
		PlayerSettings.bundleIdentifier = "com.vungle.unitytest";
		PlayerSettings.companyName = "Vungle";
		PlayerSettings.productName = "VungleUnityTest";
		PlayerSettings.bundleVersion = CommandLineReader.GetCustomArgument("ver");
		PlayerSettings.shortBundleVersion = CommandLineReader.GetCustomArgument("ver");
		
		PlayerSettings.Android.bundleVersionCode = 1;
		
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iPhone);
		var original = EditorBuildSettings.scenes; 
		
		var newSettings = new EditorBuildSettingsScene[original.Length + 1]; 
		
		System.Array.Copy(original, newSettings, original.Length); 
		
		var sceneToAdd = new EditorBuildSettingsScene("Assets/test.unity", true); 
		
		newSettings[newSettings.Length - 1] = sceneToAdd; 
		
		EditorBuildSettings.scenes = newSettings;
	}
}