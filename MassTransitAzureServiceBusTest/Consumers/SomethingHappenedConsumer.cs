using MassTransit;
using Messaging;

namespace MassTransitAzureServiceBusTest.Consumers
{
    public class SomethingHappenedConsumer : IConsumer<SomethingHappened>
    {
        private readonly ILogger<SomethingHappenedConsumer> _logger;

        public SomethingHappenedConsumer(ILogger<SomethingHappenedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SomethingHappened> context)
        {
            _logger.LogInformation("{@SomethingHappened} arrived", context.Message);

            return Task.CompletedTask;
        }
    }

    public class SomethingHappenedConsumerDefinition : ConsumerDefinition<SomethingHappenedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SomethingHappenedConsumer> consumerConfigurator, IRegistrationContext context)
        {
            base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);

            endpointConfigurator.UseEntityFrameworkOutbox<MassTransitOutboxContext>(context);
        }
    }
}
