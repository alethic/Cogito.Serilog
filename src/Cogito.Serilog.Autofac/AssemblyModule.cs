using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Autofac.Core.Registration;
using Autofac.Core.Resolving.Pipeline;

using Cogito.Autofac;
using Cogito.Serilog.Destructuring;
using Cogito.Serilog.Enrichers;

using Serilog;
using Serilog.Core;

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
            builder.RegisterModule<Cogito.Autofac.AssemblyModule>();

            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
            builder.RegisterType<DefaultLoggerConfigurationBuilder>().As<ILoggerConfigurationBuilder>();
            builder.RegisterType<DefaultLoggerConfiguratorProvider>().As<ILoggerConfiguratorProvider>();

            // default configurators
            builder.RegisterType<EnricherLoggerConfigurator>().As<ILoggerConfigurator>();
            builder.RegisterType<DestructuringPolicyConfigurator>().As<ILoggerConfigurator>();
            builder.RegisterType<ExceptionLoggerConfigurator>().As<ILoggerConfigurator>();

            // default enrichers
            builder.RegisterType<ExceptionLogContextDataEnricher>().As<ILogEventEnricher>();
            builder.RegisterType<HostEnvironmentEnricher>().As<ILogEventEnricher>();

            // default destructuring policies
            builder.RegisterType<JArrayDestructuringPolicy>().As<IDestructuringPolicy>();
            builder.RegisterType<JObjectDestructuringPolicy>().As<IDestructuringPolicy>();
            builder.RegisterType<JValueDestructuringPolicy>().As<IDestructuringPolicy>();

            // provides logger configuration
            builder.Register(ctx => ctx.Resolve<ILoggerConfigurationBuilder>().BuildConfiguration()).SingleInstance();

            // root logger instance
            builder.Register((c, p) => c.ResolveOptional<LoggerConfiguration>()?.CreateLogger() ?? Log.Logger).Named<ILogger>("").SingleInstance();

            // register actual provider for logger instances
            builder.Register(BuildLogger).As<ILogger>().ExternallyOwned();
        }

        /// <summary>
        /// Factory method to create <see cref="ILogger"/> parameters.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        ILogger BuildLogger(IComponentContext context, IEnumerable<Parameter> parameters)
        {
            // find root logger
            var logger = context.ResolveNamed<ILogger>("");

            // find parameter 
            var targetType = parameters.OfType<NamedParameter>().FirstOrDefault(np => np.Name == TargetTypeParameterName && np.Value is Type);
            if (targetType != null)
                return logger.ForContext((Type)targetType.Value);

            return logger;
        }

        /// <summary>
        /// Attaches to a component and makes an <see cref="ILogger"/> available to its registration.
        /// </summary>
        /// <param name="componentRegistry"></param>
        /// <param name="registration"></param>
        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            // ignore components that provide loggers
            if (registration.Services.OfType<TypedService>().Any(ts => ts.ServiceType == typeof(ILogger)))
                return;

            if (registration.Activator is ReflectionActivator activator)
            {
                // type does not expose a ctor accepting logger
                if (activator.ConstructorFinder
                    .FindConstructors(activator.LimitType)
                    .SelectMany(i => i.GetParameters())
                    .Any(i => i.ParameterType == typeof(ILogger)) == false)
                    return;

                registration.PipelineBuilding += (sender, builder) =>
                {
                    // attach event to inject logger
                    builder.Use(PipelinePhase.ParameterSelection, (context, next) =>
                    {
                        // discover context logger instance from registered logger
                        var logger = context.Resolve<ILogger>()?.ForContext(registration.Activator.LimitType);
                        if (logger == null)
                            throw new NullReferenceException();

                        // append logger parameter
                        context.ChangeParameters(context.Parameters.Append(TypedParameter.From(logger)));

                        // continue the resolution
                        next(context);
                    });
                };
            }
        }

    }

}
