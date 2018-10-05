using System;

using Cogito.Serilog.Enrichers;

using Serilog;
using Serilog.Configuration;
using Serilog.Context;

namespace Cogito.Serilog
{

    public static class ExceptionLogContextDataEnricherConfigurationExtensions
    {

        /// <summary>
        /// Enriches log events that contain a <see cref="Exception"/> with the <see cref="LogContext"/> captured at the time the exception is thrown.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static LoggerConfiguration WithExceptionLogContext(this LoggerEnrichmentConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            return config.With(new ExceptionLogContextDataEnricher());
        }

    }

}
