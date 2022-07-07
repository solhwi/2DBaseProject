using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using System;
using System.IO;

public class JenkinsBuildScript : MonoBehaviour
{
    public static void Build_Win64()
    {
        string currentDate = string.Format("{0:yyyy}_{0:MM}_{0:dd}_{0:HH}_{0:mm}_{0:ss}", System.DateTime.Now);
        string buildPath = string.Format("../builds/temp/{0}/", currentDate);
        string exeFileName = currentDate += ".exe";

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] {
            "Assets/Scenes/Enter.unity",
            "Assets/Scenes/Lobby.unity",
            "Assets/Scenes/Login.unity",
            "Assets/Scenes/Module.unity"
            };
        buildPlayerOptions.locationPathName = buildPath + exeFileName;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

        AddressableAssetSettings.CleanPlayerContent();
        AddressableAssetSettings.BuildPlayerContent();

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}
