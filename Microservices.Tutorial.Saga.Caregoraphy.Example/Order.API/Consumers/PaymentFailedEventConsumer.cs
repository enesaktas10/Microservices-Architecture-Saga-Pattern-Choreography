using MassTransit;
using Shared.Events;
using Stock.API.Services;
using MongoDB.Driver;
using System.Collections.Concurrent;
using Order.API.Models.Contexts;
namespace Order.API.Consumers
{
    public class PaymentFailedEventConsumer(OrderAPIDbContext _context) : IConsumer<PaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var order =await _context.Orders.FindAsync(context.Message.OrderId);

            if (order == null)
                throw new NullReferenceException();

            order.OrderStatus = Enums.OrderStatus.Completed;
            await _context.SaveChangesAsync();

        }
    }
}
