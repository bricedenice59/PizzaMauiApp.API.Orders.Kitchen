namespace PizzaMaui.API.Orders.Kitchen.Contracts
{
    public record KitchenFinishCookingEvent
    {
        public Guid CorrelationId { get; init; }
    }
}