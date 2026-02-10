


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
var app = builder.Build();

app.MapCarter();

app.Run();
