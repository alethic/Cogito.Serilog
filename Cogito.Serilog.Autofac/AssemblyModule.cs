using System;
using System.Linq;

using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Autofac.Core.Registration;

using Cogito.Autofac;

using Serilog;

namespace Cogito.Serilog.Autofac
{

    /// <summary>
    /// Provides typed instances of the Serilog Logger interface to components.
    /// </summary>
    public class AssemblyModule : ModuleBase
    {

        const string TargetTypeParameterName = "Autofac.AutowiringPropertyInjector.InstanceType";

        /// <summary>
        /// Invoked to load the module.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);

            // provides logger configuration
            builder.Register(ctx => ctx.Resolve<ILoggerConfigurationBuilder>().BuildConfiguration())
                .SingleInstance();

            // root logger instance
            builder.Register((c, p) => c.ResolveOptional<LoggerConfiguration>()?.CreateLogger() ?? Log.Logger)
                .Named<ILogger>("")
                .SingleInstance();

            // register actual provider for logger instances
            builder.Register((c, p) =>
            {
                // find root logger
                var _logger = c.ResolveNamed<ILogger>("");

                var targetType = p.OfType<NamedParameter>()
                    .FirstOrDefault(np => np.Name == TargetTypeParameterName && np.Value is Type);
                if (targetType != null)
                    return _logger.ForContext((Type)targetType.Value);

                return _logger;
            })
            .As<ILogger>()
            .ExternallyOwned();
        }

        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            // ignore components that provide loggers
            if (registration.Services.OfType<TypedService>().Any(ts => ts.ServiceType == typeof(ILogger)))
                return;

            if (registration.Activator is ReflectionActivator ra)
            {
                // find ctors that accept a logger
                var any = ra.ConstructorFinder
                    .FindConstructors(ra.LimitType)
                    .SelectMany(ctor => ctor.GetParameters())
                    .Any(pi => pi.ParameterType == typeof(ILogger));

                // no ctors found
                if (!any)
                    return;

                // attach event to inject logger
                registration.Preparing += (sender, args) =>
                {
                    // discover context logger instance from registered logger
                    var logger = args.Context.Resolve<ILogger>()?.ForContext(registration.Activator.LimitType);
                    if (logger == null)
                        throw new NullReferenceException();

                    // append logger parameter
                    args.Parameters = new[] { TypedParameter.From(logger) }.Concat(args.Parameters);
                };
            }
        }

    }

}
