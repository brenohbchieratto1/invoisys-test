using Microsoft.AspNetCore.Routing;

namespace Strategyo.Components.Api.Interfaces;

public interface IEndpoint
{
    void MapGroup(IEndpointRouteBuilder app);
}