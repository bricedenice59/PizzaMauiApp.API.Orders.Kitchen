namespace PizzaMaui.API.Orders.Kitchen.Contracts
{
    public record KitchenCanceledOrderEvent
    {
        public Guid CorrelationId { get; init; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
    }
}