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
        private LogFileChecker _logFileChecker;
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
            var logMessage =
                $"{messageObject.Time.ToShortDateString()}, " +
                $"{messageObject.Time.TimeOfDay}\t" +
                $"{messageObject.Level}\t";

            if (messageObject.ThreadId != null)
            {
                logMessage += $"{messageObject.ThreadId}\t";
            }

            if (messageObject.CallerClass != null)
            {
                logMessage += $"{messageObject.CallerClass}\t";
            }

            if (messageObject.CallerMethod != null)
            {
                logMessage += $"{messageObject.CallerMethod}\t";
            }

            if (messageObject.LineNumber != null)
            {
                logMessage += $"{messageObject.LineNumber}\t";
            }

            logMessage += $"{string.Join(", ", messageObject.Messages)}";

            if (messageObject.Ex != null)
            {
                logMessage += "\n\t" + messageObject.Ex.StackTrace;
            }

            _logWriter.WriteLine(logMessage);

            if (_cartographer.PrintToConsole)
            {
                Console.WriteLine(logMessage);
            }
        }
    }
}
