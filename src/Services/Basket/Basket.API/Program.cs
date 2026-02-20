


using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;

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
    options => {
        options.Connection(builder.Configuration.GetConnectionString("BasketDb")!);
        options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
       
    }).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = "Basket";
});
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options => 
{ options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!); });
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("BasketDb")!)
    .AddRedis(builder.Configuration["Redis:ConnectionString"]!);
builder.Services.AddMessageBroker(builder.Configuration);
var app = builder.Build();

app.MapCarter();
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
