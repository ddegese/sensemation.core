<div align="center">
  <img width="1000" height="250" alt="sensemation core" src="https://github.com/user-attachments/assets/785d80fe-757b-4624-970e-aec3bcd6141d" />

  **The lightweight middleware that turns Industrial PLCs into Web Resources.**
</div>

## The "Why"
SenseMation was born to bridge the gap between factory floor hardware and modern software ecosystems. No heavy SCADA, no proprietary locks. Sensemation.Core is the open-core acquisition engine for the Sensemation platform. It delivers the reusable contracts, runtime, configuration, logging, and plugin model that power acquisition scenarios.

## Key Features
- **Provider-Agnostic**: Plug any source (Memory, Modbus, AB) via Injectors.
- **Developer-Friendly**: Standard WebAPI for easy integration with C#, Python, or JS.
- **Extensible Architecture**: Built with SOLID principles for high-reliability environments.

---

## What Sensemation.Core is
- Modular acquisition runtime with group/trigger scheduling
- Stable contracts (DTOs) for item reads/writes
- Configuration loader + validation
- Plugin model with deterministic loading
- Memory source demo (PLC simulator)
- WebAPI adapter
- Console demo host

## What is not included (available in **Sensemation.Pro** / **Sensemation.Enterprise**)
- PLC-specific sources (Allen-Bradley, ModbusTCP, OPC-UA).
- Industrial integration tests.
- Reliable delivery, historian, and queueing.
- Advanced operational tooling and integrations (Webhooks, gRPC).

## Repository layout
```text
src/
  modules/
    Sensemation.Core.Acquisition/
      Sensemation.Core.Acquisition.Abstractions/
      Sensemation.Core.Acquisition.Runtime/
      Sensemation.Core.Acquisition.Configuration/
      Sensemation.Core.Acquisition.PluginModel/
      sources/Sensemation.Core.Acquisition.Source.Memory/
      adapters/Sensemation.Core.Acquisition.Adapter.WebApi/
      triggers/Sensemation.Core.Acquisition.Trigger.Time/
  platform/
    Sensemation.Core.Contracts/
    Sensemation.Core.Foundation/
  demo/
    Sensemation.Core.Acquisition.Demo.Console/
  tests/
    Sensemation.Core.Acquisition.UnitTests/
docs/
  architecture/
  config-examples/
```

## Quickstart (Console demo)
This repo includes a **console demo host** that runs entirely offline and wires the runtime, memory source, and WebAPI adapter together for discovery, read, and write operations.

```bash
dotnet run --project src/demo/Sensemation.Core.Acquisition.Demo.Console \
  docs/config-examples/acquisition.demo.json
```

Sample endpoints:
- `GET /api/items`
- `GET /api/items/{itemId}/latest`
- `GET /api/items/{itemId}/history?count=50`
- `POST /api/items/{itemId}/write`

## Roadmap
- [ ] Ethernet/IP & Modbus TCP Drivers.
- [ ] gRPC Streaming for real-time data.
- [ ] Persistence Adapters (SQL, InfluxDB).

## Configuration
See `docs/config-examples/acquisition.demo.json` for a minimal demo configuration. Plugin loading supports directory scan + explicit assemblies:

```json
"plugins": {
  "scanDirectories": ["./plugins"],
  "assemblies": [
    "./Sensemation.Core.Acquisition.Source.Memory.dll",
    "./Sensemation.Core.Acquisition.Adapter.WebApi.dll",
    "./Sensemation.Core.Acquisition.Trigger.Time.dll"
  ]
}
```

## Build, test, pack
```bash
./build/scripts/build.sh
./build/scripts/test.sh
./build/scripts/pack.sh
```

PowerShell equivalents are under `build/scripts/*.ps1`.

## License
Apache 2.0. Proprietary industrial connectivity and “Pro” modules are not included in this repository.
