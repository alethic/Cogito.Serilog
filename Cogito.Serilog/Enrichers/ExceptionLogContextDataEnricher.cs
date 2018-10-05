using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Cogito.Serilog.Enrichers
{

    /// <summary>
    /// Enriches log events that contain an <see cref="Exception"/> with the <see cref="LogContext"/> in scope when
    /// the <see cref="Exception"/> is thrown.
    /// </summary>
    public class ExceptionLogContextDataEnricher :
        ILogEventEnricher
    {

        /// <summary>
        /// Map of <see cref="Exception"/> instance to captured context.
        /// </summary>
        static readonly ConditionalWeakTable<Exception, ILogEventEnricher> context =
            new ConditionalWeakTable<Exception, ILogEventEnricher>();

        /// <summary>
        /// Initializes the static instances.
        /// </summary>
        static ExceptionLogContextDataEnricher()
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
        }

        /// <summary>
        /// Captures Exceptions at the context where they occur.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs args)
        {
            if (args.Exception != null)
                if (context.TryGetValue(args.Exception, out var enricher) == false)
                    context.Add(args.Exception, LogContext.Clone());
        }

        /// <summary>
        /// Enriches the log event.
        /// </summary>
        /// <param name="logEvent"></param>
        /// <param name="propertyFactory"></param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Exception != null)
                if (context.TryGetValue(logEvent.Exception, out var enricher))
                    enricher.Enrich(logEvent, propertyFactory);
        }

    }

}
