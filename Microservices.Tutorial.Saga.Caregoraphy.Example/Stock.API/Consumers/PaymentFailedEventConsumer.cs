using MassTransit;
using Shared.Events;
using Stock.API.Services;
using MongoDB.Driver;
namespace Stock.API.Consumers
{
    public class PaymentFailedEventConsumer(MongoDBServices mongoDBServices) : IConsumer<PaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var stocks = mongoDBServices.GetCollection<Models.Stock>();

            foreach (var orderItem in context.Message.OrderItems)
            {
                var stock = await (await stocks.FindAsync(s => s.ProductId == orderItem.ProductId.ToString())).FirstOrDefaultAsync();

                if (stock is not null)
                {
                    stock.Count += orderItem.Count;
                    await stocks.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId.ToString(), stock);
                }
                    
            }

        }
    }
}
