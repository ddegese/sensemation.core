// <copyright file="BaseSource.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Abstractions.Interfaces;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Abstractions.Models;

/// <summary>
/// Provides a base implementation for acquisition sources.
/// </summary>
public abstract class BaseSource : BasePlugin, ISource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSource"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="id">The source identifier.</param>
    /// <param name="parameters">The source parameters.</param>
    protected BaseSource(ILogger logger, string id, Dictionary<string, string> parameters)
        : base(logger, id, parameters)
    {
    }

    /// <inheritdoc />
    public abstract Task<Dictionary<string, DataPoint>> ReadAsync(Collection<IItemSource> items);

    /// <inheritdoc />
    public abstract Task<DataPoint> WriteAsync(IItemSource item, object value);

    /// <inheritdoc />
    protected abstract override void InitializeCore();
}
