namespace Strategyo.Results.Contracts.Results;

public partial class Result<TValue>
{
    public static implicit operator Result<TValue>(TValue value)
    {
        return new Result<TValue>(value);
    }
    
    public static implicit operator Result<TValue>((TValue value, Message message) tuple) 
        => new(tuple.value)
        {
            Messages = [tuple.message]
        };

    public static implicit operator Result<TValue>(Error error) 
        => new(error);

    public static implicit operator Result<TValue>(List<Error> errors) 
        => new(errors);

    public static implicit operator Result<TValue>(Error[] errors) 
        => new(errors.ToList());

    public static implicit operator Result<TValue>(Message message)
        => new(message);

    public static implicit operator Result<TValue>(List<Message> messages)
        => new(messages);

    public static implicit operator Result<TValue>(Message[] messages)
        => new(messages.ToList());
    
    public static implicit operator Result<TValue>((Error error, Message message) tuple)
        => new()
        {
            Errors = [tuple.error],
            Messages = [tuple.message]
        };

    public static implicit operator Result<TValue>((List<Error> errors, List<Message> messages) tuple)
        => new()
        {
            Errors = tuple.errors,
            Messages = tuple.messages
        };

    public static implicit operator Result<TValue>((Error[] errors, Message[] messages) tuple)
        => new()
        {
            Errors = tuple.errors.ToList(),
            Messages = tuple.messages.ToList()
        };
}