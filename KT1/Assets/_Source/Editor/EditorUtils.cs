using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EditorUtils : MonoBehaviour
    {
        [MenuItem("Build/Build All")]
        public static void BuildAll()
        {
            BuildWindows();
            BuildAndroid();
            BuildWebGL();

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/AutoBuild";
            
            if (Directory.Exists(path))
                Process.Start("explorer.exe", path.Replace("/", "\\"));
        }

        private static void BuildWindows()
        {
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes,
                $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop, Environment.SpecialFolderOption.None)}/AutoBuild/Windows/Game.exe",
                BuildTarget.StandaloneWindows, BuildOptions.None);
        }

        private static void BuildAndroid()
        {
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes,
                $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop, Environment.SpecialFolderOption.None)}/AutoBuild/Android/Game.apk",
                BuildTarget.Android, BuildOptions.None);
        }

        private static void BuildWebGL()
        {
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes,
                $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop, Environment.SpecialFolderOption.None)}/AutoBuild/WebGL",
                BuildTarget.WebGL, BuildOptions.None);
        }
    }
}