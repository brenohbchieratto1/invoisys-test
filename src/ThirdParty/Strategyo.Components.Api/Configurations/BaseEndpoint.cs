using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Strategyo.Components.Api.Interfaces;
using Strategyo.Results.Contracts.Results;
using HttpResult = Microsoft.AspNetCore.Http.Results;

namespace Strategyo.Components.Api.Configurations;

public abstract class BaseEndpoint : IEndpoint
{
    protected abstract string EndpointPrefix { get; }
    protected abstract string Tag { get; }
    protected virtual int Version => 1;

    public void MapGroup(IEndpointRouteBuilder app)
    {
        var group = app
                   .MapGroup($"api/v{Version}/{EndpointPrefix}")
                   .WithTags(Tag);

        MapEndpoint(group);
    }

    protected abstract void MapEndpoint(IEndpointRouteBuilder app);

    protected static IResult Result<T>(Result<T> output)
    {
        return output.HasErrors ? HttpResult.BadRequest(output) : HttpResult.Ok(output);
    }

    protected static IResult Result(Result output)
        => output.HasErrors ? HttpResult.BadRequest(output.Messages) : HttpResult.Ok(output);
}