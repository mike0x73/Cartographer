using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cartographer
{
    internal class Printer
    {
        private readonly BlockingCollection<LogMessage> _loggerQueue;
        private StreamWriter _logWriter;
        private readonly LogFileChecker _logFileChecker;
        private readonly Cartographer _cartographer;
        private readonly string _filePath;

        public Printer(Cartographer cartographer, BlockingCollection<LogMessage> loggerQueue, string filePath)
        {
            _filePath = filePath;
            _loggerQueue = loggerQueue;
            _cartographer = cartographer;
            _logFileChecker = new LogFileChecker(cartographer, filePath);

            _logWriter = new StreamWriter(filePath, true)
            {
                AutoFlush = true
            };
        }

        private LogMessage GetOldestLogMessage()
        {
            var message = _loggerQueue.Take();
            return message;
        }

        internal void QueueChecker()
        {
            while (true)
            {
                if (_cartographer.MaxFileSize > 0 && _logFileChecker.CheckFileRollover())
                {
                    _logWriter.Dispose();
                    _logFileChecker.ManageLogFile();

                    _logWriter = new StreamWriter(_filePath, true)
                    {
                        AutoFlush = true
                    };
                }

                LogMessage(GetOldestLogMessage());
            }
        }

        private void LogMessage(LogMessage messageObject)
        {
            var logMessage = new StringBuilder(10);
            logMessage.Append($"{messageObject.Time.ToShortDateString()}, ");
            logMessage.Append($"{messageObject.Time.TimeOfDay}\t");
            logMessage.Append($"{messageObject.Level}\t");

            if (messageObject.ThreadId != null)
            {
                logMessage.Append($"{messageObject.ThreadId}\t");
            }

            if (messageObject.CallerClass != null)
            {
                logMessage.Append($"{messageObject.CallerClass}\t");
            }

            if (messageObject.CallerMethod != null)
            {
                logMessage.Append($"{messageObject.CallerMethod}\t");
            }

            if (messageObject.LineNumber != null)
            {
                logMessage.Append($"{messageObject.LineNumber}\t");
            }

            logMessage.Append($"{string.Join(", ", messageObject.Messages)}");

            if (messageObject.Ex != null)
            {
                logMessage.Append($"\n{messageObject.Ex.StackTrace}");
            }

            _logWriter.WriteLine(logMessage);

            if (_cartographer.PrintToConsole)
            {
                Console.WriteLine(logMessage);
            }
        }
    }
}
