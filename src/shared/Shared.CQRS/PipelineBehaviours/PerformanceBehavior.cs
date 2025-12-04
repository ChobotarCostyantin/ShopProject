using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.CQRS.PipelineBehaviours
{
    // Shared.CQRS/PipelineBehaviours/PerformanceBehavior.cs
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var timer = Stopwatch.StartNew();
            var response = await next();
            timer.Stop();

            if (timer.ElapsedMilliseconds > 500) // Example threshold
            {
                _logger.LogWarning("Long running request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                    typeof(TRequest).Name, timer.ElapsedMilliseconds, request);
            }
            return response;
        }
    }
}