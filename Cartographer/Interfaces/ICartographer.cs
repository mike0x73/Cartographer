using Cartographer.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cartographer.Interfaces
{
    public interface ICartographer
    {
        /// <summary>
        /// Prints a log message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="loggingLevel">The logging level of the message</param>
        void Log(string message, LoggingLevel loggingLevel);

        /// <summary>
        /// Prints a log message that contains several messages.
        /// </summary>
        /// <param name="messages">The messages to log.</param>
        /// <param name="loggingLevel">The logging level of the message.</param>
        void Log(string[] messages, LoggingLevel loggingLevel);

        /// <summary>
        /// Prints a log message with an exception stack trace.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="loggingLevel">The logging level of the message.</param>
        /// <param name="ex">The exception to log.</param>
        void Log(string message, LoggingLevel loggingLevel, Exception ex);


        /// <summary>
        /// Prints a log message that contains several messages with an exception stack trace.
        /// </summary>
        /// <param name="messages">The messages to log.</param>
        /// <param name="loggingLevel">The logging level of the message.</param>
        /// <param name="ex">The exception to log.</param>
        void Log(string[] messages, LoggingLevel loggingLevel, Exception ex);

        /// <summary>
        /// Gets the current status of the logger task.
        /// </summary>
        /// <returns>The TaskStatus of the logger task.</returns>
        TaskStatus Status();
    }
}
