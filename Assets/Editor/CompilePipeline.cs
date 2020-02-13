using System.Text;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

[InitializeOnLoad]
public static  class CompilePipeline
{
    private static StringBuilder sb = new StringBuilder(512);
    private static bool compileSuc = true;
    private  static bool CompileFinished = false;

    /// <summary>
    /// 判断是否编译完成
    /// </summary>
    public static bool IsCompileFinished
    {
        get { return CompileFinished; }
    }

    static CompilePipeline()
    {
        CompilationPipeline.assemblyCompilationStarted  += OnCompilationStarted;
        CompilationPipeline.assemblyCompilationFinished += OnCompilationFinished;
    }
    
    private static void OnCompilationStarted(object obj){
        Debug.Log("编译开始");
        sb.Clear();
        compileSuc = true;
        CompileFinished = false;
    }
    
    private static void OnCompilationFinished(string str,CompilerMessage[] msgs) {
        Debug.Log("编译完成 " + str);
        if (msgs != null)
        {
            foreach (var msg in msgs)
            {
                if (msg.type == CompilerMessageType.Error)
                {
                    compileSuc = false;
                    sb.AppendFormat("{0}文件:[{1}](行号:{2},列号:{3})\n", msg.message, msg.file, msg.line, msg.column);
                }
            }
        }
        Debug.Log(GetCompileMsg());
        CompileFinished = true;
    }

    /// <summary>
    /// 获取编译消息
    /// </summary>
    /// <returns></returns>
    public static string GetCompileMsg()
    {
        if (compileSuc)
            return "compile suc!";
        return sb.ToString();
    }
}
