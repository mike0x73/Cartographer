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
        /// Adds a message to the logger queue
        /// </summary>
        /// <param name="message">The log message to add to the queue.</param>
        void Log(string message, LoggingLevel loggingLevel);

        /// <summary>
        /// Adds an array of messages to the logger queue
        /// </summary>
        /// <param name="message">The log message to add to the queue.</param>
        void Log(string[] message, LoggingLevel loggingLevel);

        /// <summary>
        /// Gets the current status of the logger task.
        /// </summary>
        /// <returns>The TaskStatus of the logger task.</returns>
        TaskStatus Status();


        /// <summary>
        /// Gets the cartographer task.
        /// </summary>
        /// <returns>The task running in the background.</returns>
        Task GetTask();
    }
}
