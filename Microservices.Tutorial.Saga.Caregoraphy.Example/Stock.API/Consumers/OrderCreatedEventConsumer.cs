using MassTransit;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer(MongoDBServices mongoDbServices,ISendEndpointProvider sendEndpointProvider,IPublishEndpoint publishEndpoint) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();
            IMongoCollection<Models.Stock> collection = mongoDbServices.GetCollection<Models.Stock>();



            //Order.API de publish ettigim OrderCreatedEvent icerisindeki OrderItemslari teker teker geziyorum
            // context.Message.OrderItems = OrderCreatedEvent ile gelen OrderItemslar
            //collection = MongoDb de stock bilgilerini tuttugum Stock entitysinin bir instance ni olusturdum ve collection uzerinden bu bilgiye erisebiliyorum
            foreach (var orderItem in context.Message.OrderItems)
            {
                stockResult.Add(await (await collection.FindAsync(s => s.ProductId == orderItem.ProductId.ToString() && s.Count >= (long)orderItem.Count)).AnyAsync());
            }


            if (stockResult.TrueForAll(s=>s.Equals(true)))
            {
                //Stock güncellemesi

                foreach (var orderItem in context.Message.OrderItems)
                {
                    Models.Stock stock =await (await collection.FindAsync(s => s.ProductId == orderItem.ProductId.ToString())).FirstOrDefaultAsync();
                    stock.Count -= orderItem.Count;

                    await collection.FindOneAndReplaceAsync(x => x.ProductId == orderItem.ProductId.ToString(), stock);

                }

                //paymenti uyaracak eventin firlatilmasi
                StockReservedEvent stockReservedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    BuyerId = context.Message.BuyerId,
                    TotalPrice = context.Message.TotalPrice,
                    OrderItems = context.Message.OrderItems,
                };

                //Daha oncesinde OrderCreatedEventte direk olarak bir publush islemi yaptik yani herhangi bir kuyruk ismi bildirmedik yayinlamis oldugumuz evente subscribe olanlarin hepsi kendi kuyruklarindan bu evente erisecekler ve gerekli islemleri yurutecekler.
                //hedef odakli direk bir servisin kuyrugunada gonderebiliiz burada biz bunu yapacagiz

               var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(
                    new Uri($"queue: {RabbitMQSettings.Payment_StockReservedEventQueue}"));

               sendEndpoint.Send(stockReservedEvent);

            }
            else
            {
                //stock islemi basarisiz.
                //Orderi uyaracak event firlatilacak

                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    Message = "Stock miktari yetersiz",
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId
                };

                await publishEndpoint.Publish(stockNotReservedEvent);
            }

        }
    }
}
