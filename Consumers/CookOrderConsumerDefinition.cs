using MassTransit;

namespace PizzaMaui.API.Orders.Kitchen.Consumers
{
    public class CookOrderConsumerDefinition :
        ConsumerDefinition<OrderIsCookingConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<OrderIsCookingConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}