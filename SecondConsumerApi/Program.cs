using MassTransit;
using Messaging;
using SecondConsumerApi.Consumers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterMassTransit(builder.Configuration, (IBusRegistrationContext context, IServiceBusBusFactoryConfigurator cfg) =>
{
    // Subscribe to OrderSubmitted directly on the topic, instead of configuring a queue
    cfg.SubscriptionEndpoint<SomethingHappened>("secondapi-message1-consumer", e =>
    {
        e.ConfigureConsumer<SomethingHappenedConsumer>(context);
    });

    // Subscribe to OrderSubmitted directly on the topic, instead of configuring a queue
    cfg.SubscriptionEndpoint<SomethingElseHappened>("secondapi-message2-consumer", e =>
    {
        e.ConfigureConsumer<SomethingElseHappenedConsumer>(context);
    });
}, Assembly.GetEntryAssembly()!, typeof(SomethingHappened).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
