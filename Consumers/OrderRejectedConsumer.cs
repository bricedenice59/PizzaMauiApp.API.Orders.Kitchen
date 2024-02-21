using MassTransit;
using PizzaMaui.API.Orders.Kitchen.Contracts;
using PizzaMauiApp.RabbitMq.Messages;

namespace PizzaMaui.API.Orders.Kitchen.Consumers
{
    public class OrderRejectedConsumer : IConsumer<IKitchenOrderRejected>
    {
        public async Task Consume(ConsumeContext<IKitchenOrderRejected> context)
        {
            // :(
            await context.Publish(new KitchenCanceledOrderEvent()
            {
                CorrelationId = Guid.NewGuid(),
                OrderId = context.Message.OrderId,
                UserId = context.Message.UserId
            });
        }
    }
}
