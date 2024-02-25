using MassTransit;
using PizzaMauiApp.RabbitMq.Messages;

namespace PizzaMaui.API.Orders.Kitchen.Consumers
{
    public class KitchenOrderCreatedConsumer : IConsumer<IOrderApiToKitchenMessage>
    {
        public async Task Consume(ConsumeContext<IOrderApiToKitchenMessage> context)
        {
            //dispatch to kitchen application's owner where he must accept or not the order
            await context.Publish<IKitchenMessage>(new
            {
                OrderId = context.Message.OrderId,
                UserId = context.Message.UserId,
                Items = context.Message.Items,
                CreatedAt = context.Message.CreatedAt
            });
        }
    }
}
