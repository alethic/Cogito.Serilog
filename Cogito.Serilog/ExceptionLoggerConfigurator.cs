using System;
using System.Collections.Generic;

using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Destructurers;

namespace Cogito.Serilog
{

    /// <summary>
    /// Configures Serilog to destructure exceptions.
    /// </summary>
    public class ExceptionLoggerConfigurator : ILoggerConfigurator
    {

        readonly IEnumerable<IExceptionDestructurer> destructurers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="destructurers"></param>
        public ExceptionLoggerConfigurator(IEnumerable<IExceptionDestructurer> destructurers)
        {
            this.destructurers = destructurers ?? throw new ArgumentNullException(nameof(destructurers));
        }

        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            return configuration
                .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                    .WithDefaultDestructurers()
                    .WithDestructurers(destructurers)
                    .WithIgnoreStackTraceAndTargetSiteExceptionFilter());
        }

    }

}
