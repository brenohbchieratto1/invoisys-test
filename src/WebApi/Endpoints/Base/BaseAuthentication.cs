using Strategyo.Components.Api.Configurations;

namespace App.InvoiSysTest.WebApi.Endpoints.Base;

public abstract class BaseAuthentication : BaseEndpoint
{
    protected override string EndpointPrefix => "authentication";
    protected override string Tag => "Authentication";
}