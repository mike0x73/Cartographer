﻿using System;
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
    /// <inheritdoc />
    public class Cartographer : ICartographer
    {
        private readonly string _filepath;
        private readonly BlockingCollection<LogMessage> _loggerQueue = new BlockingCollection<LogMessage>();
        private readonly Task _loggerTask;
        private readonly Printer _printer;

        /// <inheritdoc />
        public bool PrintToConsole { get; set; } = false;

        /// <inheritdoc />
        public LoggingLevel LoggingLevelToPrint { get; set; } = LoggingLevel.Trace;

        /// <inheritdoc />
        public bool PrintContextData { get; set; } = true;

        /// <inheritdoc />
        public bool UseStackTrace { get; set; } = false;

        /// <inheritdoc />
        public long MaxFileSize { get; set; } = 0;

        /// <inheritdoc />
        public int PaddingSize { get; set; } = 16;

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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
