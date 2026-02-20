// <copyright file="CustomLogFormatter.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Text;

using Microsoft.Extensions.Logging;

namespace Sensemation.Core.Foundation.Logging;

/// <summary>
/// Formats log messages for console output.
/// </summary>
public class CustomLogFormatter
{
    private readonly string name;
    private readonly bool includeStackTraces;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomLogFormatter"/> class.
    /// </summary>
    /// <param name="name">The logger category name.</param>
    /// <param name="includeStackTraces">Whether to include stack traces in output.</param>
    public CustomLogFormatter(string name, bool includeStackTraces = true)
    {
        this.name = name;
        this.includeStackTraces = includeStackTraces;
    }

    /// <summary>
    /// Formats a log message using the provided state and exception data.
    /// </summary>
    /// <typeparam name="TState">The log state type.</typeparam>
    /// <param name="state">The log state.</param>
    /// <param name="exception">The exception associated with the log entry, if any.</param>
    /// <param name="logLevel">The log level.</param>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="formatter">The formatter that builds the log message.</param>
    /// <returns>The formatted log message.</returns>
    public string Format<TState>(
        TState state,
        Exception? exception,
        LogLevel logLevel,
        EventId eventId,
        Func<TState, Exception?, string> formatter)
    {
        const int maxNamespaceNameSize = 40;

        ArgumentNullException.ThrowIfNull(formatter);

        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
        var categoryName = ResolveCategoryName(this.name);

        categoryName = categoryName.Length > maxNamespaceNameSize ?
            string.Concat("...", categoryName.AsSpan(categoryName.Length - (maxNamespaceNameSize - 3))) :
            categoryName.PadRight(maxNamespaceNameSize);

        var threadId = Environment.CurrentManagedThreadId;
        var logMessage = formatter(state, exception);

        var sb = new StringBuilder();
        _ = sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0} {1} thr={2,3} - ({3}) ", timestamp, categoryName, threadId, logLevel.ToString()[0]);

        var eventInfo = eventId.Id != 0 || !string.IsNullOrEmpty(eventId.Name)
            ? $"EventID: {eventId.Id} ({eventId.Name}): "
            : "EventID: 0: ";
        _ = sb.Append(eventInfo);

        _ = sb.Append(logMessage);

        // If there's an exception and we should include stack traces, add the full stack trace
        if (exception != null && this.includeStackTraces)
        {
            _ = sb.AppendLine();
            _ = sb.AppendLine(exception.ToString());
        }

        return sb.ToString();
    }

    private static string ResolveCategoryName(string categoryName)
    {
        if (string.IsNullOrEmpty(categoryName))
        {
            return string.Empty;
        }

        // Resolve generic type arguments (e.g., Logger<Namespace.Class> -> Namespace.Class)
        var openBracket = categoryName.IndexOf('<', StringComparison.Ordinal);
        var closeBracket = categoryName.LastIndexOf('>');

        if (openBracket >= 0 && closeBracket > openBracket)
        {
            var innerType = categoryName.Substring(openBracket + 1, closeBracket - openBracket - 1);
            return ResolveCategoryName(innerType);
        }

        // Handle .NET generic type name markers (e.g., MyClass`1 -> MyClass)
        var tickIndex = categoryName.IndexOf('`', StringComparison.Ordinal);
        return tickIndex >= 0 ? categoryName[..tickIndex] : categoryName;
    }
}
