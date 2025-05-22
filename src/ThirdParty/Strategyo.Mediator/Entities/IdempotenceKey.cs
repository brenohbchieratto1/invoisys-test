namespace Strategyo.Mediator.Entities;

public class IdempotenceKey
{
    public IdempotenceKey(Ulid correlationId, string inputTypeName, string outputTypeName)
    {
        CorrelationId = correlationId;
        OutputTypeName = outputTypeName;
        InputTypeName = inputTypeName;
    }

    public IdempotenceKey(Ulid correlationId, string inputTypeName)
    {
        CorrelationId = correlationId;
        OutputTypeName = "EmptyOutput";
        InputTypeName = inputTypeName;
    }

    public Ulid CorrelationId { get; }
    public string OutputTypeName { get; }
    public string InputTypeName { get; }

    public override string ToString()
        => $"{InputTypeName}_{OutputTypeName}_{CorrelationId}";
}