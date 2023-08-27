using Assets.Scripts.Utils;
using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Assets.Editor
{
	public class BuildIncrementor : IPreprocessBuildWithReport
	{
		public int callbackOrder => 1;

		public void OnPreprocessBuild(BuildReport report)
		{
			var newBuildNumber = int.Parse(PlayerSettings.macOS.buildNumber) + 1;
			PlayerSettings.macOS.buildNumber = newBuildNumber.ToString();
			try
			{
				var buildNumberRepository = ScriptableObject.CreateInstance<BuildNumberRepository>();
				buildNumberRepository.BuildNumber = newBuildNumber;
				AssetDatabase.DeleteAsset("Assets/Resources/BuildNumberRepository.asset");
				AssetDatabase.CreateAsset(buildNumberRepository, "Assets/Resources/BuildNumberRepository.asset");
				AssetDatabase.SaveAssets();
			}
			catch(Exception ex)
			{
				Debug.Log(ex.Message);
			}
		}
	}
}