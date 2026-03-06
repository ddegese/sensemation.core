// <copyright file="TriggerManager.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Demo.Console.Logging;
using Sensemation.Core.Acquisition.PluginModel.Models;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Managers;

/// <summary>
/// Manages the loading and initialization of trigger implementations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TriggerManager"/> class.
/// </remarks>
/// <param name="logger">The logger.</param>
/// <param name="serviceProvider">The service provider.</param>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by DI")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "Used by dependency injection")]
internal class TriggerManager(ILogger<TriggerManager> logger, IServiceProvider serviceProvider) : IDisposable
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA2213:Disposable fields should be disposed",
        Justification = "Owned by the DI container; TriggerManager does not create or own these dependencies.")]
    private readonly ILogger<TriggerManager> logger = logger;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA2213:Disposable fields should be disposed",
        Justification = "Owned by the DI container; TriggerManager does not create or own these dependencies.")]
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private readonly Dictionary<string, ITrigger> triggers = [];
    private readonly Dictionary<string, Type> triggerTypes = [];

    /// <summary>
    /// Initializes triggers from the provided configuration.
    /// </summary>
    /// <param name="triggerConfigs">The trigger configurations.</param>
    public void InitializeTriggers(IEnumerable<TriggerConfiguration> triggerConfigs)
    {
        ArgumentNullException.ThrowIfNull(triggerConfigs);

        foreach (var triggerConfig in triggerConfigs)
        {
            var trigger = this.CreateTrigger(triggerConfig);

            trigger.Initialize();

            this.triggers[triggerConfig.Id] = trigger;
        }
    }

    /// <summary>
    /// Loads trigger plugin descriptors.
    /// </summary>
    /// <param name="pluginDescriptors">The plugin descriptors.</param>
    public void LoadTriggerDescriptors(IEnumerable<PluginDescriptor> pluginDescriptors)
    {
        ArgumentNullException.ThrowIfNull(pluginDescriptors);

        foreach (var descriptor in pluginDescriptors)
        {
            var pluginId = descriptor.Id?.Trim();
            if (string.IsNullOrWhiteSpace(pluginId))
            {
                continue;
            }

            if (descriptor.PluginType == null)
            {
                continue;
            }

            if (!typeof(ITrigger).IsAssignableFrom(descriptor.PluginType))
            {
                continue;
            }

            this.triggerTypes[pluginId.ToUpperInvariant()] = descriptor.PluginType;
        }
    }

    /// <summary>
    /// Gets a trigger by its name.
    /// </summary>
    /// <param name="name">The name of the trigger.</param>
    /// <returns>The trigger, or null if not found.</returns>
    public ITrigger? GetTrigger(string name) => this.triggers.TryGetValue(name, out var trigger) ? trigger : null;

    /// <summary>
    /// Starts all triggers asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task StartAllTriggers()
    {
        foreach (var trigger in this.triggers.Values)
        {
            await trigger.StartAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Stops all triggers asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task StopAllTriggers()
    {
        foreach (var trigger in this.triggers.Values)
        {
            await trigger.StopAsync().ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the resources used by the TriggerManager.
    /// </summary>
    /// <param name="disposing">True if disposing, false if finalizing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var trigger in this.triggers.Values)
            {
                if (trigger is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            this.triggers.Clear();
            this.triggerTypes.Clear();
        }
    }

    /// <summary>
    /// Creates a trigger based on the configuration.
    /// </summary>
    /// <param name="triggerConfig">The trigger configuration.</param>
    /// <returns>The created trigger, or null if it could not be created.</returns>
    private ITrigger CreateTrigger(TriggerConfiguration triggerConfig)
    {
        var parameters = triggerConfig.Parameters.ToDictionary(p => p.Name, p => p.Value);
        var triggerTypeKey = triggerConfig.Type.ToUpperInvariant();

        if (!this.triggerTypes.TryGetValue(triggerTypeKey, out var triggerType))
        {
            LogMessages.TriggerNotFoundLogger(this.logger, triggerConfig.Type, triggerConfig.Id, null);
            throw new ArgumentException($"Unsupported trigger type: {triggerConfig.Type}");
        }

        var trigger = (ITrigger)ActivatorUtilities.CreateInstance(
            this.serviceProvider,
            triggerType,
            triggerConfig.Id,
            parameters);

        return trigger;
    }
}
