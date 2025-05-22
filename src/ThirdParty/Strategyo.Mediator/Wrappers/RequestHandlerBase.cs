using Microsoft.Extensions.DependencyInjection;
using Strategyo.Mediator.Interfaces;
using Strategyo.Results.Contracts.Results;

namespace Strategyo.Mediator.Wrappers;

public abstract class RequestHandlerBase
{
    public abstract Task<Result> HandleAsync(object request, IServiceProvider serviceProvider,
                                             CancellationToken cancellationToken);
}

public abstract class RequestHandlerWrapper<TResponse> : RequestHandlerBase
{
    public abstract Task<Result<TResponse>> HandleAsync(
        IRequest<TResponse> request, 
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}

public class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
    where TRequest : IRequest<TResponse>
{
    public override async Task<Result> HandleAsync(
        object request,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
        => await HandleAsync((IRequest<TResponse>)request, serviceProvider, cancellationToken).ConfigureAwait(false);

    protected TResponse Response { get; set; } = default!;
    
    public override Task<Result<TResponse>> HandleAsync(
        IRequest<TResponse> request,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var handler = serviceProvider.GetService<IRequestHandler<TRequest, TResponse>>() ?? 
                      (object?)serviceProvider.GetService<RequestHandlerWrapperImpl<TRequest, TResponse>>();

        var pipeline = serviceProvider
                      .GetServices<IPipelineBehavior<TRequest, TResponse>>()
                      .Reverse()
                      .Aggregate(
                           (RequestHandlerDelegate<Result<TResponse>>)HandlerAsync,
                           (next, behavior) => ct =>
                           {
                               var tokenToUse = ct == CancellationToken.None ? cancellationToken : ct;
                               return behavior.Handle((TRequest)request, next, tokenToUse);
                           });
        
        return pipeline(cancellationToken);

        Task<Result<TResponse>> HandlerAsync(CancellationToken ct = default)
        {
            var tokenToUse = ct == CancellationToken.None ? cancellationToken : ct;
            
            var handlerType = handler!.GetType();

            var method = handlerType.GetMethod("HandleAsync", [typeof(TRequest), typeof(CancellationToken)]);
            if (method == null)
                throw new InvalidOperationException("Método HandleAsync não encontrado no handler.");

            if (handler is RequestHandlerWrapperImpl<TRequest, TResponse> baseHandler)
            {
                var instance = Activator.CreateInstance<TResponse>();

                var prop = instance?.GetType().GetProperty("CorrelationId");

                if (prop != null)
                {
                    prop.SetValue(instance, request.CorrelationId);
                }
                
                baseHandler.Response = instance;
            }

            var result = method.Invoke(handler, [request, tokenToUse]);

            return result as Task<Result<TResponse>> ?? throw new InvalidCastException("O método HandleAsync não retornou um Task<Result<TResponse>>.");
        }
    }
}