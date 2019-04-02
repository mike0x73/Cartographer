using Cartographer.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cartographer
{
    /// <summary>
    /// I bloody hate printers irl.
    /// </summary>
    internal class Printer
    {
        private BlockingCollection<LogMessage> _loggerQueue;
        private StreamWriter _logWriter;


        public Printer(BlockingCollection<LogMessage> loggerQueue, string filepath)
        {
            _loggerQueue = loggerQueue;
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
                $"{messageObject.Time.TimeOfDay}, " +
                $"{messageObject.Level}, " +
                $"{messageObject.Caller}" +
                $"{string.Join(", ", messageObject.Messages)}";

            if (messageObject.Ex != null)
            {
                logMessage += "\n\t" + messageObject.Ex.StackTrace;
            }

            _logWriter.WriteLine(logMessage);
        }
    }
}
