# Sensemation Core Acquisition Console Demo

This demo combines the loader/manager architecture and the dynamic plugin loading approach in a simple console application.

## Configuration

By default, the configuration is resolved from the current working directory. You can also pass a path on the command line:

```bash
dotnet run --project src/demo/Sensemation.Core.Acquisition.Demo.Console \
  ./docs/config-examples/acquisition.demo.json
```

## Plugin Discovery

The demo resolves plugins by:

- Reading the `plugins.scanDirectories` and `plugins.assemblies` entries.
- Scanning the configured base path for additional `Sensemation.Core.Acquisition.*.dll` assemblies.

## Running

```bash
dotnet run --project src/demo/Sensemation.Core.Acquisition.Demo.Console
```

Press **Ctrl+C** to stop the demo. The shutdown sequence stops triggers and adapters, then disposes managers.