// <copyright file="DataPoint.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

namespace Sensemation.Core.Contracts;

/// <summary>
/// Represents a timestamped value with a quality indicator.
/// </summary>
public sealed class DataPoint
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataPoint"/> class.
    /// </summary>
    /// <param name="timestampUtc">The UTC timestamp of the value.</param>
    /// <param name="value">The value payload.</param>
    /// <param name="quality">The quality indicator for the value.</param>
    public DataPoint(DateTime timestampUtc, object? value, Quality quality)
    {
        this.TimestampUtc = timestampUtc;
        this.Value = value;
        this.Quality = quality;
    }

    /// <summary>
    /// Gets the UTC timestamp of the datapoint.
    /// </summary>
    /// <value>The UTC timestamp associated with the value.</value>
    public DateTime TimestampUtc { get; }

    /// <summary>
    /// Gets the datapoint value payload.
    /// </summary>
    /// <value>The value payload for the datapoint.</value>
    public object? Value { get; }

    /// <summary>
    /// Gets the datapoint quality indicator.
    /// </summary>
    /// <value>The quality indicator for the datapoint.</value>
    public Quality Quality { get; }

    /// <summary>
    /// Determines whether two <see cref="DataPoint"/> instances are equal.
    /// </summary>
    /// <param name="left">The left-hand operand.</param>
    /// <param name="right">The right-hand operand.</param>
    /// <returns><c>true</c> when both operands are equal; otherwise <c>false</c>.</returns>
    public static bool operator ==(DataPoint? left, DataPoint? right)
    {
        return left?.Equals(right) ?? (right is null);
    }

    /// <summary>
    /// Determines whether two <see cref="DataPoint"/> instances are not equal.
    /// </summary>
    /// <param name="left">The left-hand operand.</param>
    /// <param name="right">The right-hand operand.</param>
    /// <returns><c>true</c> when both operands are not equal; otherwise <c>false</c>.</returns>
    public static bool operator !=(DataPoint? left, DataPoint? right)
    {
        return !(left == right);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is not null &&
            obj is DataPoint other &&
            this.Quality == other.Quality &&
            Equals(this.Value, other.Value);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Quality, this.Value);
    }
}
