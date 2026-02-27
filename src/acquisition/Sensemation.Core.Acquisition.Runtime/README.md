# Sensemation.Core.Acquisition.Runtime

Core acquisition runtime package for Sensemation.

## Included libraries in this package

This package ships the runtime plus its core dependent assemblies:

- **Sensemation.Core.Acquisition.Runtime**
  - Runtime engine for groups/items and update flow
  - Runtime services such as cache and update event dispatching

- **Sensemation.Core.Acquisition.Abstractions**
  - Core interfaces and base plugin/source/trigger contracts
  - Shared acquisition model abstractions used by adapters/sources/triggers

- **Sensemation.Core.Acquisition.Configuration**
  - Configuration models and validators for acquisition setup
  - Logging/config extension helpers

- **Sensemation.Core.Acquisition.PluginModel**
  - Plugin descriptor/load options models
  - Assembly plugin loader service contracts/implementation

- **Sensemation.Core.Contracts**
  - Shared contracts such as DataPoint/Quality and serialization helpers

- **Sensemation.Core.Foundation**
  - Foundation utilities, including custom logging infrastructure

## Notes

- This package is intended as the main acquisition runtime bundle.
- Adapter/source/trigger implementations are published as separate packages.
