using System;
using System.Diagnostics;

namespace RealtimeDashboard.Core.Logging
{
	public class DebugLog : Ilog
	{
		public LogLevel LogLevel { get; private set; }

		public void Dispose()
		{
			//empty
		}

		
	
        public void WriteLine(LogLevel level, string component, string message, params object[] messageArgs)
        {
            string finalMsg = String.Format(message, messageArgs);
            finalMsg = FormatLogLine(level, component, finalMsg);
            Trace.WriteLine(finalMsg);
        }

        private static string FormatLogLine(LogLevel level, string msg)
		{
			DateTime now = DateTime.Now;
			string logLine = $"{now.ToShortDateString()} {now.ToLongTimeString()} - {level} - {msg}";
			return logLine;
		}

        private static string FormatLogLine(LogLevel level, string component, string msg)
        {
            string finalMsg = $"{component} -> {msg}";
            finalMsg = FormatLogLine(level, finalMsg);
            return finalMsg;
        }
    }
}
