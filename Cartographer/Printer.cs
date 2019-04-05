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
        private readonly Cartographer _cartographer;
        
        public Printer(Cartographer cartographer, BlockingCollection<LogMessage> loggerQueue, string filepath)
        {
            _loggerQueue = loggerQueue;
            _cartographer = cartographer;
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

            if (messageObject.Messages != null)
            {
                logMessage += $"{string.Join(", ", messageObject.Messages)}";
            }                

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
