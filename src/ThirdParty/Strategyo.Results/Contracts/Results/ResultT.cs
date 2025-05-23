using System.Diagnostics.CodeAnalysis;

namespace Strategyo.Results.Contracts.Results;

[ExcludeFromCodeCoverage]
public partial class Result<TValue> : Result
{
    public Result()
    {
        Success = false;
    }

    public Result(TValue value)
    {
        Success = true;
        Value = value;
    }

    public Result(Error error)
    {
        Errors = [error];
        Success = false;
    }

    public Result(IEnumerable<Error> errors)
    {
        Errors = [..errors];
        Success = false;
    }

    public Result(Message message)
    {
        Success = true;
        Messages = [message];
    }

    public Result(IEnumerable<Message> messages)
    {
        Success = true;
        Messages = [..messages];
    }

    public Result(Message message, Error error)
    {
        Success = false;
        Messages = [message];
        Errors = [error];
    }

    public Result(IEnumerable<Message> messages, IEnumerable<Error> errors)
    {
        Success = false;
        Messages = [..messages];
        Errors = [..errors];
    }
    
    public TValue? Value { get; set; }

    public bool TryGetErrorsAndMessages(out List<Error> errors, out List<Message> messages, out TValue data)
    {
        if (HasErrors || HasMessages)
        {
            errors = Errors;
            messages = Messages;
            data = Value!;
            return true;
        }

        errors = [];
        messages = [];
        data = Value!;
        return false;
    }

    public bool TryGetMessages(out List<Message> messages, out TValue data)
    {
        if (HasErrors || HasMessages)
        {
            messages = Messages;
            data = Value!;
            return true;
        }

        messages = [];
        data = Value!;
        return false;
    }

    public bool TryGetErrors(out List<Error> errors, out TValue data)
    {
        if (HasErrors || HasMessages)
        {
            errors = Errors;
            data = Value!;
            return true;
        }

        errors = [];
        data = Value!;
        return false;
    }

    public bool TryGetValue(out TValue? data)
    {
        if (Value == null)
        {
            data = default;
            return false;
        }
        
        data = Value!;
        return true;
    }
}