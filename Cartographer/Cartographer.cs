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
            _loggerQueue.TryAdd(new LogMessage(message, loggingLevel));
        }

        /// <inheritdoc />
        public void Log(string[] messages, LoggingLevel loggingLevel)
        {
            _loggerQueue.TryAdd(new LogMessage(messages, loggingLevel));
        }

        /// <inheritdoc />
        public void Log(string message, LoggingLevel loggingLevel, Exception ex)
        {
            _loggerQueue.TryAdd(new LogMessage(message, loggingLevel, ex));
        }

        /// <inheritdoc />
        public void Log(string[] messages, LoggingLevel loggingLevel, Exception ex)
        {
            _loggerQueue.TryAdd(new LogMessage(messages, loggingLevel, ex));
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

            if (!File.Exists(filepath))
            {
                // check for dir
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(filepath);
                }

                try
                {
                    File.Create(filepath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cartographer failed to create log file: " + ex.Message);
                }
            }
        }

        
    }
}
