﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cartographer
{
    internal class LogMessage
    {
        internal LogMessage(string message, LoggingLevel loggingLevel, ContextData contextData, int paddingSize)
        {
            Messages = new string[1];
            Messages[0] = message;
            Level = loggingLevel;
            Time = DateTime.UtcNow;
            CallerClass = contextData?.CallerClass.PadRight(paddingSize);
            CallerMethod = contextData?.CallerMethod.PadRight(paddingSize);
            LineNumber = contextData?.CallerLineNumber;
            ThreadId = contextData?.ThreadId;
        }

        internal LogMessage(string[] messages, LoggingLevel loggingLevel, ContextData contextData, int paddingSize)
        {
            Messages = messages;
            Level = loggingLevel;
            Time = DateTime.UtcNow;
            CallerClass = contextData?.CallerClass.PadRight(paddingSize);
            CallerMethod = contextData?.CallerMethod.PadRight(paddingSize);
            LineNumber = contextData?.CallerLineNumber;
            ThreadId = contextData?.ThreadId;
        }

        internal LogMessage(string message, LoggingLevel loggingLevel, Exception ex, ContextData contextData, int paddingSize)
        {
            Messages = new string[1];
            Messages[0] = message;
            Level = loggingLevel;
            Ex = ex;
            Time = DateTime.UtcNow;
            CallerClass = contextData?.CallerClass.PadRight(paddingSize);
            CallerMethod = contextData?.CallerMethod.PadRight(paddingSize);
            LineNumber = contextData?.CallerLineNumber;
            ThreadId = contextData?.ThreadId;
        }

        internal LogMessage(string[] messages, LoggingLevel loggingLevel, Exception ex, ContextData contextData, int paddingSize)
        {
            Messages = messages;
            Level = loggingLevel;
            Ex = ex;
            Time = DateTime.UtcNow;
            CallerClass = contextData?.CallerClass.PadRight(paddingSize);
            CallerMethod = contextData?.CallerMethod.PadRight(paddingSize);
            LineNumber = contextData?.CallerLineNumber;
            ThreadId = contextData?.ThreadId;
        }

        internal string[] Messages { get; }

        internal LoggingLevel Level { get; }

        internal Exception Ex { get; } = null;

        internal DateTime Time { get; }

        internal string CallerClass { get; }

        internal string CallerMethod { get; }  

        internal int? LineNumber { get; }

        internal int? ThreadId { get; }
    }
}
