using Strategyo.Components.Api.Configurations;

namespace App.InvoiSysTest.WebApi.Endpoints.Base;

public abstract class BaseOrder : BaseEndpoint
{
    protected override string EndpointPrefix => "orders";
    protected override string Tag => "Order";
}