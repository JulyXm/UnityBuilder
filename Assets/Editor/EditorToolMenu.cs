using UnityEditor;
using UnityEngine;
namespace Editor
{
    public static class EditorToolMenu
    {
        [MenuItem("打包/打包发布工具",false,101)]
        public static void BuildWindows()
        {
            BuildProject.ShowWindow();
        }
        
        [MenuItem("打包/打包Windows",false,101)]
        public static void BuildPC()
        {
            BuildProject.BuildPC();
        }
    }
}