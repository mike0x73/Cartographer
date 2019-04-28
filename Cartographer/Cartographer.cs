using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace Cartographer
{
    /// <summary>
    /// Creates a logger. Call Log() to log a message. Optional args can always be ignored.
    /// </summary>
    public class Cartographer : ICartographer
    {
        private readonly string _filepath;
        private BlockingCollection<LogMessage> _loggerQueue = new BlockingCollection<LogMessage>();
        private Task _loggerTask;
        private Printer _printer;

        /// <summary>
        /// Gets and sets whether to print logging to console (default = false).
        /// </summary>
        public bool PrintToConsole { get; set; } = false;

        /// <summary>
        /// Gets and sets what minimum logging level to print (default = Trace).
        /// </summary>
        public LoggingLevel LoggingLevelToPrint { get; set; } = LoggingLevel.Trace;

        /// <summary>
        /// Gets and sets whether to gather context data. Turn off for faster logging (default = true).
        /// </summary>
        public bool PrintContextData { get; set; } = true;

        /// <summary>
        /// Gets and sets whether to use a stacktrace to gather context data. This can be useful for debugging release builds to see how 
        /// your program has been optimised. Will decrease performance (default = false).
        /// </summary>
        public bool UseStackTrace { get; set; } = false;

        /// <summary>
        /// Gets and sets the maximum file size in bytes of the log files. Once the current log file has reached this threshhold, 
        /// it is deprecated and a new log file is created (default = 0, does not rollover).
        /// </summary>
        public long MaxFileSize { get; set; } = 0;

        /// <summary>
        /// Get and sets a custom padding size for the output log to use (default = 16). This effects the whitespace after class and method names.
        /// </summary>
        public int PaddingSize { get; set; } = 16;

        /// <summary>
        /// Sets up a new Cartographer to log anything. Spawns a new task in the background.
        /// It will try to create any directories and the log file if it does not already exist.
        /// </summary>
        /// <param name="filepath">The file path to your logfile.</param>
        public Cartographer(string filepath)
        {
            _filepath = filepath;
            SetupLogFile(_filepath);
            _printer = new Printer(this, _loggerQueue, _filepath);

            _loggerTask = Task.Factory.StartNew(() =>
            {
                _printer.QueueChecker();
            });
        }

        /// <summary>
        /// Prints a log message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="loggingLevel">The logging level of the message</param>
        public void Log(string message, LoggingLevel loggingLevel,
            [System.Runtime.CompilerServices.CallerFilePath] string classFilePath = null,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int? lineNumber = null)
        {
            if (loggingLevel < LoggingLevelToPrint)
            {
                return;
            }

            ContextData contextData = null;

            if (PrintContextData && UseStackTrace)
            {
                var stackFrame = new StackTrace(true).GetFrame(1);
                contextData = new ContextData(stackFrame);
            }

            if (PrintContextData && UseStackTrace == false)
            {
                contextData = new ContextData(classFilePath, methodName, lineNumber);
            }

            _loggerQueue.TryAdd(new LogMessage(message, loggingLevel, contextData, PaddingSize));
        }

        /// <summary>
        /// Prints a log message that contains several messages.
        /// </summary>
        /// <param name="messages">The messages to log.</param>
        /// <param name="loggingLevel">The logging level of the message.</param>
        public void Log(string[] messages, LoggingLevel loggingLevel,
            [System.Runtime.CompilerServices.CallerFilePath] string classFilePath = null,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int? lineNumber = null)
        {
            if (loggingLevel < LoggingLevelToPrint)
            {
                return;
            }

            ContextData contextData = null;

            if (PrintContextData && UseStackTrace)
            {
                var stackFrame = new StackTrace(true).GetFrame(1);
                contextData = new ContextData(stackFrame);
            }

            if (PrintContextData && UseStackTrace == false)
            {
                contextData = new ContextData(classFilePath, methodName, lineNumber);
            }

            _loggerQueue.TryAdd(new LogMessage(messages, loggingLevel, contextData, PaddingSize));
        }

        /// <summary>
        /// Prints a log message with an exception stack trace.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="loggingLevel">The logging level of the message.</param>
        /// <param name="ex">The exception to log.</param>
        public void Log(string message, LoggingLevel loggingLevel, Exception ex,
            [System.Runtime.CompilerServices.CallerFilePath] string classFilePath = null,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int? lineNumber = null)
        {
            if (loggingLevel < LoggingLevelToPrint)
            {
                return;
            }

            ContextData contextData = null;

            if (PrintContextData && UseStackTrace)
            {
                var stackFrame = new StackTrace(true).GetFrame(1);
                contextData = new ContextData(stackFrame);
            }

            if (PrintContextData && UseStackTrace == false)
            {
                contextData = new ContextData(classFilePath, methodName, lineNumber);
            }

            _loggerQueue.TryAdd(new LogMessage(message, loggingLevel, ex, contextData, PaddingSize));
        }

        /// <summary>
        /// Prints a log message that contains several messages with an exception stack trace.
        /// </summary>
        /// <param name="messages">The messages to log.</param>
        /// <param name="loggingLevel">The logging level of the message.</param>
        /// <param name="ex">The exception to log.</param>
        public void Log(string[] messages, LoggingLevel loggingLevel, Exception ex,
            [System.Runtime.CompilerServices.CallerFilePath] string classFilePath = null,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int? lineNumber = null)
        {
            if (loggingLevel < LoggingLevelToPrint)
            {
                return;
            }

            ContextData contextData = null;

            if (PrintContextData && UseStackTrace)
            {
                var stackFrame = new StackTrace(true).GetFrame(1);
                contextData = new ContextData(stackFrame);
            }

            if (PrintContextData && UseStackTrace == false)
            {
                contextData = new ContextData(classFilePath, methodName, lineNumber);
            }

            _loggerQueue.TryAdd(new LogMessage(messages, loggingLevel, ex, contextData, PaddingSize));
        }

        /// <summary>
        /// Gets the current status of the logger task.
        /// </summary>
        /// <returns>The TaskStatus of the logger task.</returns>
        public TaskStatus Status()
        {
            return _loggerTask.Status;
        }

        private void SetupLogFile(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            
            // check for dir
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
