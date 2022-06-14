using Application.Interfaces;
using MediatR;
using Serilog;

namespace Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest
        : IRequest<TResponse>
    {
        ICurrentUserService _currentUserService;

        public LoggingBehavior(ICurrentUserService currentUserService) =>
            _currentUserService = currentUserService;

        public async Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            var userName = _currentUserService.UserName;

            Log.Information("Web API Request: {Name}, UserName : {@userName}, Request: {@Request}",
                requestName, userName, request);

            var response = await next();

            return response;
        }
    }
}
