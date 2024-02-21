using MassTransit;

namespace PizzaMaui.API.Orders.Kitchen.StateMachines
{
    public class KitchenCookerStateSagaDefinition :
        SagaDefinition<KitchenCookerState>
    {
        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<KitchenCookerState> sagaConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}