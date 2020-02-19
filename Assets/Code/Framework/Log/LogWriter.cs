using System;
using System.IO;


namespace Framework
{
   /// <summary>
   /// 日志文件写入类
   /// </summary>
   public class XLogWriter
   {
      #if UNITY_EDITOR
      private readonly string _logPath = UnityEngine.Application.dataPath + "/../xlog/";
      #else
      private readonly string _logPath = UnityEngine.Application.persistentDataPath + "/xlog/";
      #endif
      private string _logFileName = "log_{0}.txt";
      private string _logFilePath;

      private FileStream _fs;
      private StreamWriter _sw;

      private Action<string, LogLevel, bool> _logWriter;
      
      private static object _lock = new object();

      public XLogWriter()
      {
         CreateLogFile();
         _logWriter = Write;
      }

      void CreateLogFile()
      {
         if (!Directory.Exists(_logPath))
            Directory.CreateDirectory(_logPath);

         
         _logFilePath = string.Concat(_logPath, string.Format(_logFileName, DateTime.Now.ToString("yyyyMMddHHmmss")));
         UnityEngine.Debug.Log("logfilepath:" + _logFilePath);
         try
         {
            _fs = new FileStream(_logFilePath,FileMode.Append,FileAccess.Write,FileShare.ReadWrite);
            _sw = new StreamWriter(_fs);
         }
         catch (Exception e)
         {
            UnityEngine.Debug.Log(e.Message);
         }
      }

      public void Release()
      {
         if (_sw != null)
         {
            _sw.Close();
            _sw.Dispose();
            _sw = null;
         }

         if (_fs != null)
         {
            _fs.Close();
            _fs.Dispose();
            _fs = null;
         }
      }

      public void WriteLog(string msg, LogLevel level, bool writeEditorLog)
      {
         _logWriter.BeginInvoke(msg,level,writeEditorLog,null,null);
      }
      private void Write(string msg, LogLevel level, bool writeEditorLog)
      {
         lock (_lock)
         {
            try
            {
               if (writeEditorLog)
               {
                  switch (level)
                  {
                     case LogLevel.Debug:
                     case LogLevel.Info:
                     {
                        UnityEngine.Debug.Log(msg);
                        break;
                     }
                     case LogLevel.Warning:
                        UnityEngine.Debug.LogWarning(msg);
                        break;
                     case LogLevel.Error:
                     case LogLevel.Fatal:
                     {
                        UnityEngine.Debug.LogError(msg);
                        break;
                     }
                        
                  }
               }
               _sw.WriteLine(msg);
               _sw.Flush();
            }
            catch (Exception e)
            {
               UnityEngine.Debug.LogError(e.Message);
            }
         }
      }

   }
    
    
}