using MassTransit;
using PizzaMaui.API.Orders.Kitchen.Contracts;
using PizzaMauiApp.RabbitMq.Messages;

namespace PizzaMaui.API.Orders.Kitchen.StateMachines
{
    public class KitchenCookerStateMachine :
        MassTransitStateMachine<KitchenCookerState> 
    {
        public KitchenCookerStateMachine()
        {
            InstanceState(x => x.CurrentState, ValidateOrder);

            Event(() => KitchenCookerEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => KitchenCanceledOrderEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => KitchenBeginCookingEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => KitchenFinishCookingEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                When(KitchenCanceledOrderEvent)
                    .Then(context =>
                    {
                        context.Saga.UserId = context.Message.UserId;
                        context.Saga.OrderId = context.Message.OrderId;
                        context.Saga.Items = context.Message.Items;
                        context.Saga.CreatedAt = context.Message.CreatedAt;
                        
                        Console.WriteLine($":( Order {context.Saga.OrderId} has been canceled...");
                        
                        context.Publish<IKitchenToOrderAPIMessage>(
                            new
                            {
                                OrderId = context.Message.OrderId,
                                UserId = context.Message.UserId,
                                CreatedAt = context.Message.CreatedAt,
                                IsAccepted = false,
                                Items = context.Message.Items
                        });
                    })
                    .Finalize()
            );
            
            Initially(
                When(KitchenCookerEvent)
                    .Then(context =>
                    {
                        context.Saga.UserId = context.Message.UserId;
                        context.Saga.OrderId = context.Message.OrderId;
                        context.Saga.Items = context.Message.Items;
                        context.Saga.CreatedAt = context.Message.CreatedAt;
                        
                        Console.WriteLine($"Order {context.Saga.OrderId} is in kitchen...");
                        
                        var beginEvent = new KitchenBeginCookingEvent()
                        {
                            CorrelationId = context.Saga.CorrelationId
                        };
                        context.Publish(beginEvent); 
                    })
                    .TransitionTo(BeginCooking)
            );
            
            During(BeginCooking,
                When(KitchenBeginCookingEvent)
                    .Then(context =>
                    {
                        context.Publish<IKitchenToOrderAPIMessage>(
                            new
                            {
                                OrderId = context.Saga.OrderId,
                                UserId = context.Saga.UserId,
                                CreatedAt = context.Saga.CreatedAt,
                                IsAccepted = true,
                                Items = context.Saga.Items
                            });
                        
                        Console.WriteLine($"Order {context.Saga.OrderId} is being cooked...");
                        var cookOrder = new CookOrder { CorrelationId = context.Saga.CorrelationId };
                        context.Publish(cookOrder); 
                    })
                    .TransitionTo(FinishCooking)
            );
            

            During(FinishCooking,
                When(KitchenFinishCookingEvent)
                    .Then(context =>
                    {
                        Console.WriteLine($"Order {context.Saga.OrderId} has been cooked... ready for delivery");
                    })
                    .TransitionTo(OrderCompleted)
            );

            SetCompletedWhenFinalized();
        }

        public State ValidateOrder { get; private set; } = null!;
        public State BeginCooking { get; private set; } = null!;
        public State FinishCooking { get; private set; } = null!;
        public State OrderCompleted { get; private set; } = null!;

        public Event<KitchenOrderEvent> KitchenCookerEvent { get; private set; }  = null!;
        public Event<KitchenCanceledOrderEvent> KitchenCanceledOrderEvent { get; private set; } = null!;
        public Event<KitchenBeginCookingEvent> KitchenBeginCookingEvent { get; private set; } = null!;
        public Event<KitchenFinishCookingEvent> KitchenFinishCookingEvent { get; private set; } = null!;
    }
}