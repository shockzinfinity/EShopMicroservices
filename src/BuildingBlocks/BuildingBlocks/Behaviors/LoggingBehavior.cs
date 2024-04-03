using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
  : IPipelineBehavior<TRequest, TResponse>
  where TRequest : notnull, IRequest<TResponse> // IRequest 이므로 모든 request 에 적용
  where TResponse : notnull
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}", typeof(TRequest).Name, typeof(TResponse).Name, request);

    var timer = new Stopwatch();
    timer.Start();

    var response = await next();

    timer.Stop();
    var timeTaken = timer.Elapsed;
    if (timeTaken.Seconds > 3)
    {
      logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken}", typeof(TRequest).Name, timeTaken.Seconds);
    }

    logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest), typeof(TResponse));

    return response;
  }
}