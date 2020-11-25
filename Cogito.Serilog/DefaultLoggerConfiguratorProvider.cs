using System;
using System.Collections.Generic;

namespace Cogito.Serilog
{

    /// <summary>
    /// Provides the default <see cref="ILoggerConfigurator"/> instances in the container registry.
    /// </summary>
    public class DefaultLoggerConfiguratorProvider : ILoggerConfiguratorProvider
    {

        readonly IEnumerable<ILoggerConfigurator> configuration;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="configuration"></param>
        public DefaultLoggerConfiguratorProvider(IEnumerable<ILoggerConfigurator> configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IEnumerable<ILoggerConfigurator> GetConfigurators()
        {
            return configuration;
        }

    }

}
