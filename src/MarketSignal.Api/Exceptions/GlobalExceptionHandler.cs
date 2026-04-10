using MarketSignal.Core.Instrument.Spec;

using Microsoft.AspNetCore.Diagnostics;

namespace MarketSignal.Api.Exceptions;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger
) : IExceptionHandler {

    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext ctx, Exception exception, CancellationToken cancelToken) {
        var (status, message) = exception switch {
            InvalidOperationException e => (StatusCodes.Status400BadRequest, ""),
            KeyNotFoundException e => (StatusCodes.Status404NotFound, ""),
            UnsupportedInstrumentSpecException e => (StatusCodes.Status400BadRequest, e.Message),
            _ => (StatusCodes.Status500InternalServerError, "Internal server error")
        };

        if (status == 500) {
            _logger.LogError(exception, "Internal server error");
        }

        ctx.Response.StatusCode = status;
        await ctx.Response.WriteAsJsonAsync(new { error = message }, cancelToken);
        return true;
    }

}