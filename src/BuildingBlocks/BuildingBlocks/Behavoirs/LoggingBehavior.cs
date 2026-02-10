using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace BuildingBlocks.Behavoirs
{
    public class LoggingBehavior<TRequest, TResponse>(
        ILogger<LoggingBehavior<TRequest, TResponse>> _logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private const int PerformanceThresholdMs = 500;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var requestId = Guid.NewGuid().ToString();

            _logger.LogInformation(
                "[{RequestId}] Starting request {RequestName}",
                requestId,
                requestName);

            // Log request details in debug mode
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(
                    "[{RequestId}] Request details: {RequestData}",
                    requestId,
                    SerializeRequest(request));
            }

            TResponse? response = default;
            var timer = Stopwatch.StartNew();

            try
            {
                response = await next();

                timer.Stop();

                _logger.LogInformation(
                    "[{RequestId}] Completed request {RequestName} in {ElapsedMs}ms",
                    requestId,
                    requestName,
                    timer.ElapsedMilliseconds);

                // Performance warning
                if (timer.ElapsedMilliseconds > PerformanceThresholdMs)
                {
                    _logger.LogWarning(
                        "[{RequestId}] Long-running request {RequestName} took {ElapsedMs}ms (threshold: {ThresholdMs}ms)",
                        requestId,
                        requestName,
                        timer.ElapsedMilliseconds,
                        PerformanceThresholdMs);
                }

                // Log response in debug mode
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug(
                        "[{RequestId}] Response details: {ResponseData}",
                        requestId,
                        SerializeResponse(response));
                }

                return response;
            }
            catch (Exception ex)
            {
                timer.Stop();

                _logger.LogError(
                    ex,
                    "[{RequestId}] Request {RequestName} failed after {ElapsedMs}ms",
                    requestId,
                    requestName,
                    timer.ElapsedMilliseconds);

                throw;
            }
        }

        private string SerializeRequest(TRequest request)
        {
            try
            {
                return JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    WriteIndented = false,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });
            }
            catch
            {
                return request.ToString() ?? string.Empty;
            }
        }

        private string SerializeResponse(TResponse response)
        {
            try
            {
                return JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    WriteIndented = false,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });
            }
            catch
            {
                return response.ToString() ?? string.Empty;
            }
        }
    }
}