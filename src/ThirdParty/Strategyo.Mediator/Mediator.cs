using System.Collections.Concurrent;
using Strategyo.Mediator.Interfaces;
using Strategyo.Mediator.Wrappers;
using Strategyo.Results.Contracts.Results;

namespace Strategyo.Mediator;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    private static readonly ConcurrentDictionary<Type, RequestHandlerBase> RequestHandlers = [];
    
    public Task<Result<TResponse>> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handler = (RequestHandlerWrapper<TResponse>)RequestHandlers.GetOrAdd(request.GetType(), static requestType =>
        {
            var wrapperType = typeof(RequestHandlerWrapperImpl<,>).MakeGenericType(requestType, typeof(TResponse));
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper type for {requestType}");
            return (RequestHandlerBase)wrapper;
        });

        return handler.HandleAsync(request, serviceProvider, cancellationToken);
    }

    public Task<Result> SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        var handler = (RequestHandlerWrapper<Result>)RequestHandlers.GetOrAdd(request.GetType(), static requestType =>
        {
            var wrapperType = typeof(RequestHandlerWrapperImpl<,>).MakeGenericType(requestType, typeof(Result));
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper type for {requestType}");
            return (RequestHandlerBase)wrapper;
        });

        return handler.HandleAsync(request, serviceProvider, cancellationToken);
    }
}