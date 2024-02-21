using MassTransit;
using PizzaMauiApp.RabbitMq.Messages;

namespace PizzaMaui.API.Orders.Kitchen.Consumers
{
    public class OrderCreatedConsumer : IConsumer<IOrderMessage>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        
        public OrderCreatedConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        
        public async Task Consume(ConsumeContext<IOrderMessage> context)
        {
            //dispatch to kitchen application's owner where he must accept or not the order
            await _publishEndpoint.Publish<IKitchenMessage>(new
            {
                OrderId = context.Message.OrderId,
                UserId = context.Message.UserId,
                Items = context.Message.Items
            });
        }
    }
}
