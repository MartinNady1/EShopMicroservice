using BuildingBlocks.Behavoirs;
using Carter;
using FluentValidation;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(
    cfg => {
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); 
        cfg.AddOpenBehavior(typeof(ValidateBehavior<,>));
        cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    }); 
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddCarter(new DependencyContextAssemblyCatalog([typeof(Program).Assembly]));
builder.Services.AddMarten(
    options => options.Connection(builder.Configuration.GetConnectionString("CatalogDb")!)).UseLightweightSessions();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("CatalogDb")!);
var app = builder.Build();

app.MapCarter();
app.UseHealthChecks("/health" , 
    new HealthCheckOptions 
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
