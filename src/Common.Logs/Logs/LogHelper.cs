using System;
using System.Diagnostics;

namespace Common.Logs
{
    public interface ILogHelper
    {
        void Log(string message, int level);
    }

    public class LogHelper : ILogHelper
    {
        public void Log(string message, int level = 1)
        {
            var logMessage = string.Format("{0} [{1}]{2} {3}", "LogCenter.Common.LogHelper", "Default", level.ToString(), message);
            Trace.WriteLine(logMessage);
        }

        #region for extensions and simple use

        private static readonly Lazy<LogHelper> _lazy = new Lazy<LogHelper>(() => new LogHelper());
        public static ILogHelper Instance => ServiceLocator.Current.GetService<ILogHelper>() ?? _lazy.Value;
        public static void Debug(string message)
        {
            Instance.Debug(message);
        }

        #endregion
    }
}
