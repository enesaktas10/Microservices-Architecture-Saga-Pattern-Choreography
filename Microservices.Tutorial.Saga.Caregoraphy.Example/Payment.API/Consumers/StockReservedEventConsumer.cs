using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<StockReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            if (true)
            {
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId
                };

                await publishEndpoint.Publish(paymentCompletedEvent);
                await Console.Out.WriteLineAsync("Odeme basarili");
            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    Message = "Yetersiz bakiye",
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems
                };

                await publishEndpoint.Publish(paymentFailedEvent);
                await Console.Out.WriteLineAsync("Odeme basarisiz");
            }

            
        }
    }
}
