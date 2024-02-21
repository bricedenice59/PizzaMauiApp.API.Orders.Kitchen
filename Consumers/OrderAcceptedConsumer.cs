using MassTransit;
using PizzaMaui.API.Orders.Kitchen.Contracts;
using PizzaMauiApp.RabbitMq.Messages;

namespace PizzaMaui.API.Orders.Kitchen.Consumers
{
    public class OrderAcceptedConsumer : IConsumer<IKitchenOrderAccepted>
    {
        public async Task Consume(ConsumeContext<IKitchenOrderAccepted> context)
        {
            //let's cook
            await context.Publish(new KitchenOrderEvent()
            {
                CorrelationId = Guid.NewGuid(),
                Items = context.Message.Items,
                OrderId = context.Message.OrderId,
                UserId = context.Message.UserId
            });
        }
    }
}
