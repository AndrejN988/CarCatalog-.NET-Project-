using System.Threading.Tasks;
using Car.Inventory.Service.Models;
using MassTransit;
using Play.Catalog.Concrats;
using Play.Common;

namespace Car.Inventory.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> repository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;

            var item = await repository.GetAsync(message.ItemId);

            if (item == null) 
            {
                item = new CatalogItem
                {
                    Id = message.ItemId,
                    Name = message.Name,
                    Description = message.Description
                };
                await repository.CreateAsync(item);
            }
            else{

                item.Name = message.Name;
                item.Description = message.Description;

                await repository.UpdateAsync(item);

            }

            

            
        }
    }
}