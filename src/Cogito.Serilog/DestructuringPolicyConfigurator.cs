using System;
using System.Collections.Generic;
using System.Linq;

using Serilog;
using Serilog.Core;

namespace Cogito.Serilog
{

    /// <summary>
    /// Applies registered <see cref="IDestructuringPolicy"/> instances.
    /// </summary>
    public class DestructuringPolicyConfigurator : ILoggerConfigurator
    {

        readonly IEnumerable<IDestructuringPolicy> policies;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="policies"></param>
        public DestructuringPolicyConfigurator(IEnumerable<IDestructuringPolicy> policies)
        {
            this.policies = policies ?? throw new ArgumentNullException(nameof(policies));
        }

        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            return configuration.Destructure.With(policies.ToArray());
        }

    }

}
