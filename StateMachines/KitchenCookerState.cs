using MassTransit;
using PizzaMauiApp.RabbitMq.Messages;

namespace PizzaMaui.API.Orders.Kitchen.StateMachines
{
    public class KitchenCookerState :
        SagaStateMachineInstance 
    {
        public int CurrentState { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid OrderId { get; set; }
        public required string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<IOrderItem> Items { get; set; } = new();
    }
}