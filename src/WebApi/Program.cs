using App.InvoisysTest.Application.Extensions;
using App.InvoisysTest.Infrastructure.Extensions;
using Strategyo.Components.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerStrategyo("App.InvoisysTest");

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerStrategyo();
}

app.UseHttpsRedirection();

await app.RunAsync();