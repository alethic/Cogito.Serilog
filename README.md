# Cogito.Serilog

[![Build](https://github.com/alethic/Cogito.Serilog/actions/workflows/Cogito.Serilog.yml/badge.svg)](https://github.com/alethic/Cogito.Serilog/actions/workflows/Cogito.Serilog.yml)

Composes a Serilog logger from contributions, so any assembly can add an enricher or sink without editing startup.

## Packages

**[Cogito.Serilog](https://www.nuget.org/packages/Cogito.Serilog)** — Compose a Serilog `LoggerConfiguration` from contributions rather than a single builder call.

**[Cogito.Serilog.Autofac](https://www.nuget.org/packages/Cogito.Serilog.Autofac)** — Builds the Serilog `ILogger` inside an Autofac container, from the configurators the container holds.

Each package carries its own README with the detail; the links above go to nuget.org.

## Building

```shell
dotnet restore Cogito.Serilog.sln
dotnet msbuild -p:Configuration=Release Cogito.Serilog.dist.msbuildproj
```

Packages are staged into `dist/nuget` and test suites into `dist/tests`; run a suite with
`dotnet test -f <tfm> <path to its assembly>`.

## License

MIT — see [LICENSE](LICENSE).
