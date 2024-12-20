using System;
using System.Collections.Generic;

using Serilog;
using Serilog.Core;

namespace Cogito.Serilog
{

    /// <summary>
    /// Applies any registered enrichers to the logger.
    /// </summary>
    public class EnricherLoggerConfigurator : ILoggerConfigurator
    {

        readonly IEnumerable<ILogEventEnricher> enrichers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="enrichers"></param>
        public EnricherLoggerConfigurator(IEnumerable<ILogEventEnricher> enrichers)
        {
            this.enrichers = enrichers ?? throw new ArgumentNullException(nameof(enrichers));
        }

        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            foreach (var enricher in enrichers)
                configuration = configuration.Enrich.With(enricher);

            return configuration;
        }

    }

}
