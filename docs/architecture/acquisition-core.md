# Sensemation.Core Acquisition Architecture

```
Sources -> Runtime -> Adapters
    \       |       /
     \ Update Pipe /
      Triggers -> Groups
```

## Flow overview
- **Sources** read raw values from PLCs or simulations.
- **Triggers** schedule polling.
- **Groups** pair a source and trigger, then poll on schedule.
- **Runtime** owns items, cache, and update pipeline.
- **Adapters** subscribe to runtime updates and expose APIs.

## Item cache + freshness
- Every item stores recent values in a thread-safe cache.
- Each cache entry records `TimestampUtc` and `Quality` metadata.
- Groups track `LastRefreshUtc`, `LastSuccessUtc`, `LastErrorUtc`, and `ConsecutiveFailures` for observability.

## Update event pipeline
- Sources emit updates into a single `UpdateEventDispatcher`.
- A background consumer applies updates, refreshes cache entries, and notifies subscribers.
- This replaces fire-and-forget task storms and centralizes error handling.

## Plugin loading
- Plugins can be discovered by scanning directories.
- Tests and deterministic runs can list explicit assemblies in config.