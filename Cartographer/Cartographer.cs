using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Concurrent;
using Cartographer.Interfaces;
using Cartographer.Messages;
using System.Diagnostics;

namespace Cartographer
{
    /// <summary>
    /// Creates a logger and logger queue for the program. The file directory setup depends on computer OS.
    /// </summary>
    public class Cartographer : ICartographer
    {
        private readonly string _filepath;
        private BlockingCollection<LogMessage> _loggerQueue = new BlockingCollection<LogMessage>();
        private Task _loggerTask;
        private Printer _printer;

        /// <summary>
        /// Sets up a new Cartographer to log anything. Spawns a new task in the background.
        /// It will try to create any directories and the log file if it does not already exist.
        /// </summary>
        /// <param name="filepath">The file path to your logfile.</param>
        public Cartographer(string filepath)
        {
            _filepath = filepath;
            SetupLogFile(filepath);
            _printer = new Printer(_loggerQueue, _filepath);

            _loggerTask = Task.Factory.StartNew(() =>
            {
                _printer.QueueChecker();
            });
        }

        /// <inheritdoc />
        public void Log(string message, LoggingLevel loggingLevel)
        {
            var stack = new StackTrace();
            var callerClass = stack.GetFrame(1).GetMethod().DeclaringType.FullName;
            var callerMethod = stack.GetFrame(1).GetMethod().Name;
            _loggerQueue.TryAdd(new LogMessage(message, loggingLevel, callerClass, callerMethod));
        }

        /// <inheritdoc />
        public void Log(string[] messages, LoggingLevel loggingLevel)
        {
            var stack = new StackTrace();
            var callerClass = stack.GetFrame(1).GetMethod().DeclaringType.FullName;
            var callerMethod = stack.GetFrame(1).GetMethod().Name;
            _loggerQueue.TryAdd(new LogMessage(messages, loggingLevel, callerClass, callerMethod));
        }

        /// <inheritdoc />
        public void Log(string message, LoggingLevel loggingLevel, Exception ex)
        {
            var stack = new StackTrace();
            var callerClass = stack.GetFrame(1).GetMethod().DeclaringType.FullName;
            var callerMethod = stack.GetFrame(1).GetMethod().Name;
            _loggerQueue.TryAdd(new LogMessage(message, loggingLevel, ex, callerClass, callerMethod));
        }

        /// <inheritdoc />
        public void Log(string[] messages, LoggingLevel loggingLevel, Exception ex)
        {
            var stack = new StackTrace();
            var callerClass = stack.GetFrame(1).GetMethod().DeclaringType.FullName;
            var callerMethod = stack.GetFrame(1).GetMethod().Name;
            _loggerQueue.TryAdd(new LogMessage(messages, loggingLevel, ex, callerClass, callerMethod));
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
