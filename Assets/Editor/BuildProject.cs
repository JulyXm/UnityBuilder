using System;
using UnityEditor;
using UnityEngine;
using UniGameTools.BuildMechine;
using UniGameTools.BuildMechine.BuildActions;

namespace Editor
{
    public class BuildProject : EditorWindow
    {
        public static BuildProject Instance;

        public static void ShowWindow()
        {
            if (Instance == null)
            {
                var window = GetWindow<BuildProject>(false, "ProjectBuild", false);
                window.minSize = new Vector2(860f,660f);
                window.Show();
                Instance = window;
            }
            else
            {
                Instance.Close();
            }
        }

        private void OnEnable()
        {
            Instance = this;
        }

        private void OnGUI()
        {
            
        }


        public static void BuildPC()
        {
            BuildMechine.NewPipeline()
                .AddActions(new BuildAction_SetScriptingDefineSymbols(BuildTargetGroup.Standalone, "TEST"))
                .AddActions(new BuildAction_IncreaseBuildNum())
                .AddActions(new BuildAction_SaveAndRefresh(),
                    new BuildAction_SetBundleId("cn.test.test"),
                    new BuildAction_BuildProjectWindowsStandalone("game", "Game","Build/Windows/",  true)
                )
                .Run();
        }
    }
}