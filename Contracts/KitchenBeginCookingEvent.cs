using PizzaMauiApp.RabbitMq.Messages;

namespace PizzaMaui.API.Orders.Kitchen.Contracts
{
    public record KitchenBeginCookingEvent
    {
        public Guid CorrelationId { get; init; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public List<IOrderItem> Items { get; set; } = new();
    }
}