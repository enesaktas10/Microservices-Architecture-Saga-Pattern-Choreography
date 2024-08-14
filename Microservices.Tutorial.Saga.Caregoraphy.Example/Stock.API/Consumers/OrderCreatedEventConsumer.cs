using MassTransit;
using MongoDB.Driver;
using Shared.Events;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer(MongoDBServices mongoDbServices) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();
            IMongoCollection<Models.Stock> collection = mongoDbServices.GetCollection<Models.Stock>();

            foreach (var orderItem in context.Message.OrderItems)
            {
                stockResult.Add(await (await collection.FindAsync(s => s.ProductId == orderItem.ProductId && s.Count >= orderItem.Count)).AnyAsync());
            }

            if (stockResult.TrueForAll(s=>s.Equals(true)))
            {
                //Stock güncellemesi

                //paymenti uyaracak eventin firlatilmasi

            }
            else
            {
                //stock islemi basarisiz.
                //Orderi uyaracak event firlatilacak
            }

        }
    }
}
