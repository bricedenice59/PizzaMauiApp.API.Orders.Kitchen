 namespace PizzaMaui.API.Orders.Kitchen.Contracts
{
    public record CookOrder
    {
        public Guid CorrelationId { get; set; }
    }
}