using System;

namespace RealtimeDashboard.Core.Logging
{
	public enum LogLevel
	{
		Debug,
		Info,
		Warning,
		Error,
		Critical
	}

	public interface Ilog : IDisposable
	{
		LogLevel LogLevel { get; }
        void WriteLine(LogLevel level, string component, string message, params object[] messageArgs);
    }
}
