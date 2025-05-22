using Strategyo.Mediator.Entities;

namespace Strategyo.Mediator.Interfaces;

public interface IBaseRequest;

public interface IRequest : IBaseRequest
{
    IdempotenceKey IdempotenceKey { get; }
    bool IgnoreIdempotence { get; }
    public Ulid CorrelationId { get; set; }
    public void SetCorrelationId(Ulid correlationId);
    public void SetCorrelationId(string correlationId);
}

public interface IRequest<out TResponse> : IRequest;

