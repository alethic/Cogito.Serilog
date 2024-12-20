using Microsoft.Extensions.Hosting;

using Serilog.Core;
using Serilog.Events;

namespace Cogito.Serilog.Enrichers
{

    /// <summary>
    /// Enriches the logger with some basic hosting information.
    /// </summary>
    public class HostEnvironmentEnricher : ILogEventEnricher
    {

        readonly IHostEnvironment env;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="env"></param>
        public HostEnvironmentEnricher(IHostEnvironment env = null)
        {
            this.env = env;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (env == null)
                return;
            if (env.ApplicationName != null)
                logEvent.AddPropertyIfAbsent(new LogEventProperty("ApplicationName", new ScalarValue(env.ApplicationName)));
            if (env.EnvironmentName != null)
                logEvent.AddPropertyIfAbsent(new LogEventProperty("EnvironmentName", new ScalarValue(env.EnvironmentName)));
        }

    }

}
