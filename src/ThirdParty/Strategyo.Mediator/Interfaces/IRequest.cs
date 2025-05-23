using Strategyo.Mediator.Entities;

namespace Strategyo.Mediator.Interfaces;

public interface IBaseRequest;

public interface IRequest : IBaseRequest
{
    IdempotenceKey IdempotenceKey { get; }
    bool IgnoreIdempotence { get; }
    public Guid CorrelationId { get; set; }
    public void SetCorrelationId(Guid correlationId);
    public void SetCorrelationId(string correlationId);
}

public interface IRequest<out TResponse> : IRequest;

