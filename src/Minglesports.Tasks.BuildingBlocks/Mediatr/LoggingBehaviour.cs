using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Minglesports.Tasks.BuildingBlocks.Mediatr
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // request
            _logger.LogInformation("Start executing [{Request}] operation handler", request.GetType().Name);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var response = await next();

            stopwatch.Stop();

            // response
            _logger.LogInformation("Stop executing [{Request}] operation handler. Time elapsed = [{Time}] ms",
                request.GetType().Name, stopwatch.ElapsedMilliseconds);
            return response;
        }
    }
}