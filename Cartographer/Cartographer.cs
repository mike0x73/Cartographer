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
    /// Creates a logger and logger queue for the program.
    /// </summary>
    public class Cartographer : ICartographer
    {
        private readonly string _filepath;
        private BlockingCollection<LogMessage> _loggerQueue = new BlockingCollection<LogMessage>();
        private Task _loggerTask;
        private Printer _printer;

        /// <inheritdoc />
        public bool PrintToConsole { get; set; } = false;

        /// <inheritdoc />
        public LoggingLevel LoggingLevelToPrint { get; set; } = LoggingLevel.Trace;

        /// <inheritdoc />
        public bool PrintContextData { get; set; } = true;

        /// <summary>
        /// Sets up a new Cartographer to log anything. Spawns a new task in the background.
        /// It will try to create any directories and the log file if it does not already exist.
        /// </summary>
        /// <param name="filepath">The file path to your logfile.</param>
        public Cartographer(string filepath)
        {
            _filepath = filepath;
            SetupLogFile(filepath);
            _printer = new Printer(this, _loggerQueue, _filepath);

            _loggerTask = Task.Factory.StartNew(() =>
            {
                _printer.QueueChecker();
            });
        }

        /// <inheritdoc />
        public void Log(string message, LoggingLevel loggingLevel)
        {
            if (loggingLevel < LoggingLevelToPrint)
            {
                return;
            }

            ContextData contextData = null;

            if (PrintContextData)
            {
                var stackFrame = new StackTrace(true).GetFrame(1);
                contextData = new ContextData(stackFrame);
            }

            _loggerQueue.TryAdd(new LogMessage(message, loggingLevel, contextData));
        }

        /// <inheritdoc />
        public void Log(string[] messages, LoggingLevel loggingLevel)
        {
            if (loggingLevel < LoggingLevelToPrint)
            {
                return;
            }

            ContextData contextData = null;

            if (PrintContextData)
            {
                var stackFrame = new StackTrace(true).GetFrame(1);
                contextData = new ContextData(stackFrame);
            }

            _loggerQueue.TryAdd(new LogMessage(messages, loggingLevel, contextData));
        }

        /// <inheritdoc />
        public void Log(string message, LoggingLevel loggingLevel, Exception ex)
        {
            if (loggingLevel < LoggingLevelToPrint)
            {
                return;
            }

            ContextData contextData = null;

            if (PrintContextData)
            {
                var stackFrame = new StackTrace(true).GetFrame(1);
                contextData = new ContextData(stackFrame);
            }

            _loggerQueue.TryAdd(new LogMessage(message, loggingLevel, ex, contextData));
        }

        /// <inheritdoc />
        public void Log(string[] messages, LoggingLevel loggingLevel, Exception ex)
        {
            if (loggingLevel < LoggingLevelToPrint)
            {
                return;
            }

            ContextData contextData = null;

            if (PrintContextData)
            {
                var stackFrame = new StackTrace(true).GetFrame(1);
                contextData = new ContextData(stackFrame);
            }

            _loggerQueue.TryAdd(new LogMessage(messages, loggingLevel, ex, contextData));
        }

        /// <inheritdoc />
        public TaskStatus Status()
        {
            return _loggerTask.Status;
        }

        private void SetupLogFile(string filepath)
        {
            // seperate file from path
            var file = filepath.Split('\\').Last();
            var dir = filepath.Substring(0, filepath.Length - file.Length);

            // check for dir
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
