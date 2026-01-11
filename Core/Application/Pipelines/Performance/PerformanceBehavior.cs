using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Core.Application.Pipelines.Performance
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, IIntervalRequest
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            string requestName = request.GetType().Name;
            var stopwatch = Stopwatch.StartNew();

            TResponse response;

            try
            {
                stopwatch.Start();
                response = await next();
            }
            finally
            {
                if (stopwatch.Elapsed.TotalMilliseconds > request.Interval)
                {
                    string message = $"Performance -> {requestName} {stopwatch.Elapsed.TotalMilliseconds} ms";

                    Debug.WriteLine(message);
                    _logger.LogInformation(message);
                }

                stopwatch.Restart();
            }

            return response;
        }
    }
}
