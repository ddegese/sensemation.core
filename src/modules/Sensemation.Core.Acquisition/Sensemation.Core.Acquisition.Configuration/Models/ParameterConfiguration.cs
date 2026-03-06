// <copyright file="ParameterConfiguration.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Configuration.Models;

/// <summary>
/// Represents a named configuration parameter.
/// </summary>
public class ParameterConfiguration
{
    /// <summary>
    /// Gets or sets the parameter name.
    /// </summary>
    /// <value>The parameter name.</value>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the parameter value.
    /// </summary>
    /// <value>The parameter value.</value>
    public string Value { get; set; } = string.Empty;
}
