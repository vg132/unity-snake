using System.IO;
using UnityEditor;
using UnityEngine;
using System;

namespace Assets.Editor
{
	public class BuildPlayerWithVersion : MonoBehaviour
	{
		[MenuItem("Build/Build With Version")]
		public static void MyBuild()
		{
			// Log some of the current build options retrieved from the Build Settings Window
			var buildPlayerOptions = BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(new BuildPlayerOptions());
			var fullPath = buildPlayerOptions.locationPathName.Replace("\\", "/");
			var fileName = Path.GetFileName(fullPath);
			var path = fullPath.Substring(0, fullPath.Length - fileName.Length);
			path = Path.Join(path, PlayerSettings.productName, DateTime.UtcNow.ToString("yyyy-MM-dd HH_mm_ss"));
			Directory.CreateDirectory(path);
			var outputPath = path;
			path = Path.Join(path, fileName);

			buildPlayerOptions = new BuildPlayerOptions
			{
				assetBundleManifestPath = buildPlayerOptions.assetBundleManifestPath,
				extraScriptingDefines = buildPlayerOptions.extraScriptingDefines,
				locationPathName = path,
				options = buildPlayerOptions.options,
				scenes = buildPlayerOptions.scenes,
				subtarget = buildPlayerOptions.subtarget,
				target = buildPlayerOptions.target,
				targetGroup = buildPlayerOptions.targetGroup
			};

			Debug.Log("Building with the following settings\n"
				+ "Scenes: " + string.Join(",", buildPlayerOptions.scenes) + "\n"
				+ "Build location: " + buildPlayerOptions.locationPathName + "\n"
				+ "Options: " + buildPlayerOptions.options + "\n"
				+ "Target: " + buildPlayerOptions.target);

			BuildPipeline.BuildPlayer(buildPlayerOptions);
			outputPath = outputPath.Replace("/", "\\");
			var basePath = outputPath.Substring(0, outputPath.LastIndexOf("\\"));
			Directory.Move(outputPath, $"{basePath}\\{buildPlayerOptions.target} - {PlayerSettings.macOS.buildNumber}");
		}
	}
}