using PizzaMauiApp.RabbitMq.Messages;

namespace PizzaMaui.API.Orders.Kitchen.Contracts
{
    public record KitchenCanceledOrderEvent
    {
        public Guid CorrelationId { get; init; }
        public Guid OrderId { get; set; }
        public required string UserId { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public List<IOrderItem> Items { get; set; } = new();
    }
}