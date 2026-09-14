# Cogito.Serilog

Compose a Serilog `LoggerConfiguration` from contributions rather than a single builder call.

## Why

Serilog is configured in one fluent chain at startup. That means an assembly cannot add an enricher
or a destructuring policy of its own without editing that chain — so logging concerns end up in the
host instead of with the code they belong to.

## Install

```shell
dotnet add package Cogito.Serilog
```

## Contributing

Implement `ILoggerConfigurator` and the builder applies it:

```csharp
[RegisterAs(typeof(ILoggerConfigurator))]
public class MyEnricher : ILoggerConfigurator
{
    public void Apply(LoggerConfiguration configuration) =>
        configuration.Enrich.WithProperty("Component", "Import");
}
```

`ILoggerConfigurationBuilder` assembles every contributor into the final configuration, and
`ILoggerConfiguratorProvider` is how they are discovered.

## In the box

- `ExceptionLogContextDataEnricher` — promotes data attached to an exception's `Data` into log
  properties, so context gathered at the throw site survives to the sink.
- `HostEnvironmentEnricher` — adds machine and environment properties.
- `JObjectDestructuringPolicy`, `JArrayDestructuringPolicy`, `JValueDestructuringPolicy` — log
  Newtonsoft JSON values as structured data instead of as `ToString()`.
- `DelegateLogEventSink` — a sink from a lambda, useful in tests.

## License

MIT.
