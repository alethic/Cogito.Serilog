using System.Collections.Generic;

namespace Cogito.Serilog
{

    /// <summary>
    /// Describes a class that will provide configurations.
    /// </summary>
    public interface ILoggerConfiguratorProvider
    {

        /// <summary>
        /// Gets the available configuration configurators.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ILoggerConfigurator> GetConfigurators();

    }

}
