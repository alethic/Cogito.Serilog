# Cogito.Serilog.Autofac

Builds the Serilog `ILogger` inside an Autofac container, from the configurators the container holds.

## Why

So that an assembly can contribute an enricher, sink or destructuring policy by registering it, and
the logger the application resolves already has it — no startup edit.

## Install

```shell
dotnet add package Cogito.Serilog.Autofac
```

## Use

```csharp
builder.RegisterAllAssemblyModules();
```

`ILogger` then resolves from the container, configured by every registered `ILoggerConfigurator`.
Pair with `Cogito.Extensions.Logging.Serilog.Autofac` to have `ILogger<T>` write to it as well.

## License

MIT.
