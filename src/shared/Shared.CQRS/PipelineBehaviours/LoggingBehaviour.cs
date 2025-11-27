using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.CQRS.PipelineBehaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
    
    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next(cancellationToken);

        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3)
        {
            _logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds", 
                typeof(TRequest).Name, timeTaken.Seconds);
        }
        
        _logger.LogInformation("[END] Handler {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);

        return response;
    }
}