using Brickmakers.AspSecurityHeaders.Controllers.Models;
using Microsoft.Extensions.Logging;

namespace Brickmakers.AspSecurityHeaders.Logging;

/// <summary>
///     Extension methods for <see cref="ILogger" />
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    ///     Logs the given <see cref="CspReport" />, including all report fields as properties on the log entry.
    /// </summary>
    /// <param name="logger">The extended <see cref="ILogger" /> instance to log the message to.</param>
    /// <param name="cspReport">The report to be logged</param>
    /// <param name="level">
    ///     Optional parameter for the log level the csp report should be logged with. By default, it is logged
    ///     with the <see cref="LogLevel.Error" /> level.
    /// </param>
    /// <remarks>
    ///     Internally, the method uses <see cref="ILogger.BeginScope{TState}" /> to log the properties by creating a
    ///     dictionary of them. See https://nblumhardt.com/2016/11/ilogger-beginscope/zero for more details on how this works.
    ///     When using structured logging, you will get extra properties of the CSP report. Otherwise, you will only see a
    ///     summarizing message.
    /// </remarks>
    public static void LogCspReport(this ILogger logger, CspReport cspReport, LogLevel level = LogLevel.Error)
    {
        using var scope = logger.BeginScope(cspReport.ToDictionary());
        logger.Log(level, "{CspReport}", cspReport);
    }
}