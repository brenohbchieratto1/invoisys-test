using System.Diagnostics.CodeAnalysis;

namespace Strategyo.Results.Contracts.Results;

[ExcludeFromCodeCoverage]
public partial class Result
{
    public Result()
    {
        Success = false;
    }

    public Result(Error error)
    {
        Success = false;
        Errors = [error];
    }

    public Result(IEnumerable<Error> errors)
    {
        Success = false;
        Errors = [..errors];
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

    public List<Message> Messages { get; set; } = [];
    public List<Error> Errors { get; set; } = [];

    public bool Success { get; set; }
    public bool Failure => !Success;
    
    public bool HasErrors => Errors.Count > 0;
    public bool HasMessages => Messages.Count > 0;
}