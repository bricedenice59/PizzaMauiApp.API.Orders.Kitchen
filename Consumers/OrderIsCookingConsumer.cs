using MassTransit;
using PizzaMaui.API.Orders.Kitchen.Contracts;

namespace PizzaMaui.API.Orders.Kitchen.Consumers
{
    public class OrderIsCookingConsumer : IConsumer<CookOrder>
    {
        public async Task Consume(ConsumeContext<CookOrder> context)
        {
            var random = new Random();
            var rDouble = random.Next(6_0000,120_000);
            await Task.Delay(rDouble);
            await context.Publish(
                new KitchenFinishCookingEvent
                {
                    CorrelationId = context.Message.CorrelationId
                });
        }
    }
}