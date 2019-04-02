﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cartographer.Messages
{
    internal class LogMessage
    {
        internal LogMessage(string message, LoggingLevel loggingLevel, string classMember, string callerMember)
        {
            Messages = new string[1];
            Messages[0] = message;
            Level = loggingLevel;
            Time = DateTime.UtcNow;
            CallerClass = classMember;
            CallerMethod = callerMember;
        }

        internal LogMessage(string[] messages, LoggingLevel loggingLevel, string classMember, string callerMember)
        {
            Messages = messages;
            Level = loggingLevel;
            Time = DateTime.UtcNow;
            CallerClass = classMember;
            CallerMethod = callerMember;
        }

        internal LogMessage(string message, LoggingLevel loggingLevel, Exception ex, string classMember, string callerMember)
        {
            Messages = new string[1];
            Messages[0] = message;
            Level = loggingLevel;
            Ex = ex;
            Time = DateTime.UtcNow;
            CallerClass = classMember;
            CallerMethod = callerMember;
        }

        internal LogMessage(string[] messages, LoggingLevel loggingLevel, Exception ex, string classMember, string callerMember)
        {
            Messages = messages;
            Level = loggingLevel;
            Ex = ex;
            Time = DateTime.UtcNow;
            CallerClass = classMember;
            CallerMethod = callerMember;
        }

        internal string[] Messages { get; }

        internal LoggingLevel Level { get; }

        internal Exception Ex { get; } = null;

        internal DateTime Time { get; }

        internal string CallerClass { get; }

        internal string CallerMethod { get; }        
    }
}
