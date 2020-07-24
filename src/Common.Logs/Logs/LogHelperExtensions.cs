using System;

namespace Common.Logs
{
    public static class LogHelperExtensions
    {
        public static void Trace(this ILogHelper logHelper, string message)
        {
            logHelper.Log(message, 0);
        }
        public static void Debug(this ILogHelper logHelper, string message)
        {
            logHelper.Log(message, 1);
        }
        public static void Info(this ILogHelper logHelper, string message)
        {
            logHelper.Log(message, 2);
        }
        public static void Warn(this ILogHelper logHelper, string message)
        {
            logHelper.Log(message, 3);
        }
        public static void Error(this ILogHelper logHelper, Exception ex, string message)
        {
            var logMessage = string.Format("{0}=>{1}", message, ex?.StackTrace);
            logHelper.Log(logMessage, 4);
        }
        public static void Fatal(this ILogHelper logHelper, Exception ex, string message)
        {
            var logMessage = string.Format("{0}=>{1}", message, ex?.StackTrace);
            logHelper.Log(logMessage, 5);
        }
    }
}