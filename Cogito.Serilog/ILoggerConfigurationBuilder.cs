using Serilog;

namespace Cogito.Serilog
{

    /// <summary>
    /// Describes a class that will produce the logger configuration.
    /// </summary>
    public interface ILoggerConfigurationBuilder
    {

        /// <summary>
        /// Builds the configuration root.
        /// </summary>
        /// <returns></returns>
        LoggerConfiguration BuildConfiguration();

    }

}