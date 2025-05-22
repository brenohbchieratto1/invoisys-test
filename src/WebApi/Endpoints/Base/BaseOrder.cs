using Strategyo.Components.Api.Configurations;

namespace App.InvoisysTest.WebApi.Endpoints.Base;

public abstract class BaseOrder : BaseEndpoint
{
    protected override string EndpointPrefix => "order";
    protected override string Tag => "Order";
}