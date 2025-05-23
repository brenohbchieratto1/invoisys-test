namespace Strategyo.Mediator.Entities;

public class IdempotenceKey
{
    public IdempotenceKey(Guid correlationId, string inputTypeName, string outputTypeName)
    {
        CorrelationId = correlationId;
        OutputTypeName = outputTypeName;
        InputTypeName = inputTypeName;
    }

    public IdempotenceKey(Guid correlationId, string inputTypeName)
    {
        CorrelationId = correlationId;
        OutputTypeName = "EmptyOutput";
        InputTypeName = inputTypeName;
    }

    public Guid CorrelationId { get; }
    public string OutputTypeName { get; }
    public string InputTypeName { get; }

    public override string ToString()
        => $"{InputTypeName}_{OutputTypeName}_{CorrelationId}";
}