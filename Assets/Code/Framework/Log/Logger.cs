using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 日志控制类
    /// </summary>
    public static class XLogger
    {
        public static LogLevel AllLogLv = LogLevel.Debug | LogLevel.Info | LogLevel.Warning | LogLevel.Error |
                                       LogLevel.Fatal;

        public const bool ShowStack = true;
        private static XLogWriter _logWriter;

        static XLogger()
        {
            _logWriter = new XLogWriter();
            Application.logMessageReceived += HandleLog;
        }

        public static void Shutdown()
        {
            _logWriter.Release();
            _logWriter = null;
        }

        
        public static void Debug(string msg, bool showStack = ShowStack)
        {
            if(LogLevel.Debug == (AllLogLv & LogLevel.Debug))
                Log(string.Concat(" [DEBUG]: ",msg,showStack ? GetStackInfo():""),LogLevel.Debug);
        }
        
        public static void Info(string msg, bool showStack = ShowStack)
        {
            if(LogLevel.Info == (AllLogLv & LogLevel.Info))
                Log(string.Concat(" [INFO]: ",msg,showStack ? GetStackInfo():""),LogLevel.Info);
        }
        
        public static void Warning(string msg, bool showStack = ShowStack)
        {
            if(LogLevel.Warning == (AllLogLv & LogLevel.Warning))
                Log(string.Concat(" [WARNING]: ",msg,showStack ? GetStackInfo():""),LogLevel.Warning);
        }
        
        public static void Error(string msg, bool showStack = ShowStack)
        {
            if(LogLevel.Error == (AllLogLv & LogLevel.Error))
                Log(string.Concat(" [ERROR]: ",msg,showStack ? GetStackInfo():""),LogLevel.Error);
        }
        
        private static void HandleLog(string message, string stacktrace, LogType logType)
        {
            var logLevel = LogLevel.Debug;
            switch (logType)
            {
                case LogType.Assert:
                    logLevel = LogLevel.Debug;
                    break;
                case LogType.Error:
                    logLevel = LogLevel.Fatal;
                    break;
                case LogType.Exception:
                    logLevel = LogLevel.Error;
                    break;
                case LogType.Log:
                    logLevel = LogLevel.Info;
                    break;
                case LogType.Warning:
                    logLevel = LogLevel.Warning;
                    break;
            }

            if (logLevel == (AllLogLv & logLevel))
            {
                Log(string.Concat("[SYS_", logLevel, "]: ", message, "\n", stacktrace), logLevel, false);
            }
        }
        
        /// <summary>
        /// 获取堆栈信息
        /// </summary>
        /// <returns></returns>
        static string GetStackInfo()
        {
            StringBuilder sb = new StringBuilder();
            StackTrace st = new StackTrace();
            var sf = st.GetFrames();
            if (sf != null)
            {
                sb.Append("\n");
                for (int i = 1; i < sf.Length; i++)
                {
                    System.Reflection.MethodBase method = sf[i].GetMethod();
                    sb.AppendFormat("[CALL STACK][{0}]: {1}.{2}\n", i, method.DeclaringType.FullName, method.Name);
                }

                return sb.ToString();
            }

            return string.Empty;
        }
 
        private static void Log(string message, LogLevel level, bool writeEditorLog = true)
        {
            var msg = string.Concat(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"), message);
            _logWriter.WriteLog(msg,level,writeEditorLog);
        }
        
    }
}