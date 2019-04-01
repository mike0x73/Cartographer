using System;
using System.Collections.Generic;
using System.Text;

namespace Cartographer.Messages
{
    public class LogMessage
    {
        public LogMessage(string message, LoggingLevel loggingLevel)
        {
            Messages = new string[1];
            Messages[0] = message;
            Level = loggingLevel;
            Time = DateTime.UtcNow;
        }

        public LogMessage(string[] messages, LoggingLevel loggingLevel)
        {
            Messages = messages;
            Level = loggingLevel;
            Time = DateTime.UtcNow;
        }

        public LogMessage(string message, LoggingLevel loggingLevel, Exception ex)
        {
            Messages = new string[1];
            Messages[0] = message;
            Level = loggingLevel;
            Ex = ex;
            Time = DateTime.UtcNow;
        }

        public LogMessage(string[] messages, LoggingLevel loggingLevel, Exception ex)
        {
            Messages = messages;
            Level = loggingLevel;
            Ex = ex;
            Time = DateTime.UtcNow;
        }

        public string[] Messages { get; }

        public LoggingLevel Level { get; }

        public Exception Ex { get; } = null;

        public DateTime Time { get; }
    }
}
