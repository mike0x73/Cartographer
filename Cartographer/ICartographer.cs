using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cartographer
{
    public interface ICartographer
    {
        /// <summary>
        /// Gets and sets whether to print logging to console (default = false).
        /// </summary>
        bool PrintToConsole { get; set; }
        
        /// <summary>
        /// Gets and sets what minimum logging level to print (default = Trace).
        /// </summary>
        LoggingLevel LoggingLevelToPrint { get; set; }
        
        /// <summary>
        /// Gets and sets whether to gather context data. Turn off for faster logging (default = true).
        /// </summary>
        bool PrintContextData { get; set; }

        /// <summary>
        /// Gets and sets whether to use a stacktrace to gather context data. This can be useful for debugging release builds to see how 
        /// your program has been optimised. Will decrease performance (default = false).
        /// </summary>
        bool UseStackTrace { get; set; }

        /// <summary>
        /// Gets and sets the maximum file size in bytes of the log files. Once the current log file has reached this threshhold, 
        /// it is deprecated and a new log file is created (default = 0, does not rollover).
        /// </summary>
        long MaxFileSize { get; set; }

        /// <summary>
        /// Get and sets a custom padding size for the output log to use (default = 16). This effects the whitespace after class and method names.
        /// </summary>
        int PaddingSize { get; set; }

        /// <summary>
        /// Prints a log message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="loggingLevel">The logging level of the message</param>
        void Log(string message, LoggingLevel loggingLevel,
            [System.Runtime.CompilerServices.CallerFilePath] string classFilePath = null,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int? lineNumber = null);

        /// <summary>
        /// Prints a log message that contains several messages.
        /// </summary>
        /// <param name="messages">The messages to log.</param>
        /// <param name="loggingLevel">The logging level of the message.</param>
        void Log(string[] messages, LoggingLevel loggingLevel,
            [System.Runtime.CompilerServices.CallerFilePath] string classFilePath = null,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int? lineNumber = null);

        /// <summary>
        /// Prints a log message with an exception stack trace.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="loggingLevel">The logging level of the message.</param>
        /// <param name="ex">The exception to log.</param>
        void Log(string message, LoggingLevel loggingLevel, Exception ex,
            [System.Runtime.CompilerServices.CallerFilePath] string classFilePath = null,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int? lineNumber = null);


        /// <summary>
        /// Prints a log message that contains several messages with an exception stack trace.
        /// </summary>
        /// <param name="messages">The messages to log.</param>
        /// <param name="loggingLevel">The logging level of the message.</param>
        /// <param name="ex">The exception to log.</param>
        void Log(string[] messages, LoggingLevel loggingLevel, Exception ex,
            [System.Runtime.CompilerServices.CallerFilePath] string classFilePath = null,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int? lineNumber = null);

        /// <summary>
        /// Gets the current status of the logger task.
        /// </summary>
        /// <returns>The TaskStatus of the logger task.</returns>
        TaskStatus Status();
    }
}
