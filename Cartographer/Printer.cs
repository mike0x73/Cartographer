using Cartographer.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cartographer
{
    internal class Printer
    {
        private BlockingCollection<LogMessage> _loggerQueue;
        private StreamWriter _logWriter;
        private readonly bool _printToConsole;
        
        public Printer(BlockingCollection<LogMessage> loggerQueue, string filepath, bool printToConsole)
        {
            _loggerQueue = loggerQueue;
            _printToConsole = printToConsole;
            _logWriter = new StreamWriter(filepath, true)
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
                LogMessage(GetOldestLogMessage());
            }
        }

        private void LogMessage(LogMessage messageObject)
        {
            var logMessage =
                $"{messageObject.Time.ToShortDateString()}, " +
                $"{messageObject.Time.TimeOfDay}\t" +
                $"{messageObject.Level}\t";

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

            if (messageObject.Messages != null)
            {
                logMessage += $"{string.Join(", ", messageObject.Messages)}";
            }                

            if (messageObject.Ex != null)
            {
                logMessage += "\n\t" + messageObject.Ex.StackTrace;
            }

            _logWriter.WriteLine(logMessage);

            if (_printToConsole)
            {
                Console.WriteLine(logMessage);
            }
        }
    }
}
