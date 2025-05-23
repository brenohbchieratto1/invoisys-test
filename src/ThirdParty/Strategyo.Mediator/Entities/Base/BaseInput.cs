using Strategyo.Mediator.Interfaces;

namespace Strategyo.Mediator.Entities.Base;

public abstract class BaseInput<TInputType> : IRequest
    where TInputType : notnull
{
    public string? LogUser { get; set; }

    public Guid CorrelationId { get; set; }
    public virtual IdempotenceKey IdempotenceKey => new(CorrelationId, typeof(TInputType).Name);
    public virtual bool IgnoreIdempotence => false;

    public void SetCorrelationId(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    
    public void SetCorrelationId(string correlationId)
    {
        CorrelationId = Guid.Parse(correlationId);
    }

    public void SetLogUser(string? logUser)
    {
        LogUser = logUser;
    }
}

public abstract class BaseInput<TInputType, TOutputType> : BaseInput<TInputType>, IRequest<TOutputType>
    where TInputType : notnull
{
    public override IdempotenceKey IdempotenceKey => new(CorrelationId, typeof(TInputType).Name, typeof(TOutputType).Name);
}