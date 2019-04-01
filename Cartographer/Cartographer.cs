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
        private StreamWriter _logWriter;
        private readonly string _filepath;
        private BlockingCollection<LogMessage> _loggerQueue = new BlockingCollection<LogMessage>();
        private Task _loggerTask;

        /// <summary>
        /// Sets up a new Cartographer to log anything. Spawns a new task in the background.
        /// It will try to create any directories and the log file if it does not already exist.
        /// </summary>
        /// <param name="filepath">The file path to your logfile.</param>
        public Cartographer(string filepath)
        {
            _filepath = filepath;
            SetupLogFile(filepath);
            _logWriter = new StreamWriter(_filepath, true)
            {
                AutoFlush = true
            };

            _loggerTask = Task.Factory.StartNew(() =>
            {
                QueueChecker();
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

        public void Log(string message, LoggingLevel loggingLevel, Exception ex)
        {
            _loggerQueue.TryAdd(new LogMessage(message, loggingLevel, ex));
        }

        public void Log(string[] message, LoggingLevel loggingLevel, Exception ex)
        {
            _loggerQueue.TryAdd(new LogMessage(message, loggingLevel, ex));
        }
        
        /// <inheritdoc />
        public TaskStatus Status()
        {
            return _loggerTask.Status;
        }

        /// <inheritdoc />
        public Task GetTask()
        {
            return _loggerTask;
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
                    try
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cartographer failed to create directory to log file: " + ex.Message);
                    }
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

        private LogMessage GetOldestLogMessage()
        {
            var message = _loggerQueue.Take();
            return message;
        }

        private void QueueChecker()
        {
            while (true)
            {
                LogMessage(GetOldestLogMessage());
            }
        }

        private void LogMessage(LogMessage messageObject)
        {
            var logMessage =
                $"{messageObject.Time.ToShortDateString()}, " +
                $"{messageObject.Time.TimeOfDay}, " +
                $"{messageObject.Level}, " +
                $"{string.Join(", ", messageObject.Messages)}";

            if (messageObject.Ex != null)
            {
                logMessage += "\n\t" + messageObject.Ex.StackTrace;
            }

            _logWriter.WriteLine(logMessage);
        }
    }
}
