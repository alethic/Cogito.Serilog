using System;
using System.Collections.Generic;
using System.Linq;

using Cogito.Autofac;

using Microsoft.Extensions.Configuration;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Cogito.Serilog.Autofac
{

    /// <summary>
    /// Builds the default configuration.
    /// </summary>
    [RegisterAs(typeof(ILoggerConfigurationBuilder))]
    public class DefaultLoggerConfigurationBuilder : ILoggerConfigurationBuilder
    {

        readonly IEnumerable<ILoggerConfiguratorProvider> providers;
        readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="providers"></param>
        public DefaultLoggerConfigurationBuilder(IConfiguration configuration = null, IEnumerable<ILoggerConfiguratorProvider> providers = null)
        {
            this.configuration = configuration;
            this.providers = providers ?? Enumerable.Empty<ILoggerConfiguratorProvider>();
        }

        /// <summary>
        /// Builds the configuration.
        /// </summary>
        /// <returns></returns>
        public LoggerConfiguration BuildConfiguration()
        {
            var config = BuildDefaultConfiguration();
            config = ApplyDefaults(config);
            config = ApplyConfigurations(config);
            return config;
        }

        /// <summary>
        /// Builds the default <see cref="LoggerConfiguration"/> instance.
        /// </summary>
        /// <returns></returns>
        LoggerConfiguration BuildDefaultConfiguration()
        {
            return new LoggerConfiguration();
        }

        /// <summary>
        /// Builds a default configuration.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        LoggerConfiguration ApplyDefaults(LoggerConfiguration builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder = builder
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Console(theme: SystemConsoleTheme.Literate)
                .WriteTo.Trace();

            // append configuration if presented
            if (configuration != null)
                builder = builder.ReadFrom.Configuration(configuration);

            return builder;
        }

        /// <summary>
        /// Applies extra configuration from the builder configuration configurators.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        LoggerConfiguration ApplyConfigurations(LoggerConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            foreach (var provider in providers)
                foreach (var configuration in provider.GetConfigurators())
                    config = configuration.Apply(config);

            return config;
        }

    }

}
