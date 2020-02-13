using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

namespace UniGameTools.BuildMechine.BuildActions
{
    public class BuildAction_BuildProjectWindowsStandalone : BuildAction
    {
        public bool X64;
        public string ExeName;
        public string BuildPath;
        public string ProjectName;

        public BuildAction_BuildProjectWindowsStandalone(string projectName, string exeName, string buildPath, bool x64)
        {
            ProjectName = projectName;
            X64 = x64;
            ExeName = exeName;
            BuildPath = buildPath;
        }

        public override BuildState OnUpdate()
        {
            var listScene = BuildHelper.GetAllScenesInBuild();

            var fileName = ExeName;

            if (ExeName.EndsWith(".exe") == false)
            {
                fileName += ".exe";
            }

            var x = X64 ? "x64" : "x86";

            var dirName = string.Format("{0}_{3}_build{1}_{2:yyyyMMddHHmm}", this.ProjectName, BuildHelper.GetBuildNum(), DateTime.Now, x);

            BuildHelper.AddBuildNum();

            var path = Path.Combine(BuildPath, dirName);

            path = Path.Combine(path, fileName);

            var buildTarget = X64 ? BuildTarget.StandaloneWindows64 : BuildTarget.StandaloneWindows;
                
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = listScene.ToArray();
            buildPlayerOptions.locationPathName = path;
            buildPlayerOptions.target = buildTarget;
            buildPlayerOptions.options = BuildOptions.None;
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Context.Set("BuildPath", path);
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
                return BuildState.Success;
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError("Build failed");
                throw new Exception("Build Fail");
            }
            return BuildState.Failure;
        }

        public override BuildProgress GetProgress()
        {
            return null;
        }
    }
}