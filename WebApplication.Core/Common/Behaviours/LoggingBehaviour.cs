using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication.Core.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {

        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling {requestName}", typeof(TRequest).Name);

            var startTime = Stopwatch.GetTimestamp();
            var response = await next();
            var elapsedTime = Stopwatch.GetElapsedTime(startTime);

            _logger.LogInformation("Handled {requestName}. Completed in: {elapsedTime}", typeof(TRequest).Name, elapsedTime);
            return response;
        }
    }
}
