namespace Framework
{
    /// <summary>
    /// 日志系统的设计
    /// 1.能够按照日志的等级输出
    /// 2.支持输出保存到文件
    /// 3.支持显示到unity窗口中
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }
    
    
}