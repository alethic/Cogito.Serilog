using System;

using Serilog.Core;
using Serilog.Events;

namespace Cogito.Serilog
{

    /// <summary>
    /// Sends log events to an action.
    /// </summary>
    public class DelegateLogEventSink :
        ILogEventSink
    {

        readonly Action<LogEvent> action;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        public DelegateLogEventSink(Action<LogEvent> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Emit(LogEvent logEvent)
        {
            action(logEvent);
        }

    }

}
