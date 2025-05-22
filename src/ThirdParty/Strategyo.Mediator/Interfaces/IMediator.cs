using Strategyo.Results.Contracts.Results;

namespace Strategyo.Mediator.Interfaces;

public interface IMediator
{
    Task<Result<TResponse>> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task<Result> SendAsync(IRequest request, CancellationToken cancellationToken = default);
}