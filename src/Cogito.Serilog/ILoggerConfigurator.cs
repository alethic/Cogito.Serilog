using Serilog;

namespace Cogito.Serilog
{

    /// <summary>
    /// Provides additional configuration for the <see cref="LoggerConfiguration"/> setup.
    /// </summary>
    public interface ILoggerConfigurator
    {

        /// <summary>
        /// Applies the additional configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        LoggerConfiguration Apply(LoggerConfiguration configuration);

    }

}
