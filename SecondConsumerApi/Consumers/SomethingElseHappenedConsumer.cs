using MassTransit;
using Messaging;

namespace SecondConsumerApi.Consumers
{
    public class SomethingElseHappenedConsumer : IConsumer<SomethingElseHappened>
    {
        private readonly ILogger<SomethingHappenedConsumer> _logger;

        public SomethingElseHappenedConsumer(ILogger<SomethingHappenedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SomethingElseHappened> context)
        {
            _logger.LogInformation("{@SomethingHappened} arrived", context.Message);

            return Task.CompletedTask;
        }
    }

    public class SomethingElseHappenedConsumerDefinition : ConsumerDefinition<SomethingElseHappenedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SomethingElseHappenedConsumer> consumerConfigurator, IRegistrationContext context)
        {
            base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);

            endpointConfigurator.UseEntityFrameworkOutbox<MassTransitOutboxContext>(context);
        }
    }
}
