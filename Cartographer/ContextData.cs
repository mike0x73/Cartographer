using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Cartographer
{
    internal class ContextData
    {
        internal ContextData(StackFrame stackFrame)
        {
            CallerClass = stackFrame.GetMethod().DeclaringType.Name;
            CallerMethod = stackFrame.GetMethod().Name;
            CallerLineNumber = stackFrame.GetFileLineNumber();
            ThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        internal string CallerClass { get; }

        internal string CallerMethod { get; }

        internal int CallerLineNumber { get; }

        internal int ThreadId { get; }
    }
}
