namespace Strategyo.Results.Contracts.Results;

public partial class Result
{
    public static implicit operator Result(Error error)
        => new(error);

    public static implicit operator Result(List<Error> errors)
        => new(errors);

    public static implicit operator Result(Error[] errors)
        => new(errors.ToList());
    
    public static implicit operator Result(Message message)
        => new(message);

    public static implicit operator Result(List<Message> messages)
        => new(messages);

    public static implicit operator Result(Message[] messages)
        => new(messages.ToList());
    
    public static implicit operator Result((Error error, Message message) tuple)
        => new()
        {
            Errors = [tuple.error],
            Messages = [tuple.message]
        };

    public static implicit operator Result((List<Error> errors, List<Message> messages) tuple)
        => new()
        {
            Errors = tuple.errors,
            Messages = tuple.messages
        };

    public static implicit operator Result((Error[] errors, Message[] messages) tuple)
        => new()
        {
            Errors = tuple.errors.ToList(),
            Messages = tuple.messages.ToList()
        };

    public static implicit operator Result(bool value)
    {
        return new Result
        {
            Success = value
        };
    }
}