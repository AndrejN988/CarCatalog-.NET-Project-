using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Car.Service.Catalog.Dtos;
using Car.Service.Catalog.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Concrats;
using Play.Common;

namespace Car.Service.Catalog.Controller
{
    [ApiController]
    [Route("items")]
    public class CarItemsController : ControllerBase
    {

        private readonly IRepository<CarItem> itemsRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public CarItemsController(IRepository<CarItem> itemsRepository, IPublishEndpoint publishEndpoint)
        {
            this.itemsRepository = itemsRepository;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IEnumerable<CarItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync()).Select(items => items.AsDto());
            return items;
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CarItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null) return NotFound();

            return item.AsDto();
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<CarItemDto>> PostAsync(CreateCarItemDto createCarItemDto)
        {
            var item = new CarItem
            {
                Name = createCarItemDto.Name,
                Description = createCarItemDto.Description,
                Price = createCarItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);

            await publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);

        }

        // PUT /items/{id}
        [HttpPut]
        public async Task<IActionResult> PutAsync(Guid id, UpdateCarItemDto updateCarItemDto)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateCarItemDto.Name;
            existingItem.Description = updateCarItemDto.Description;
            existingItem.Price = updateCarItemDto.Price;

            await itemsRepository.UpdateAsync(existingItem);

            await publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));

            return NoContent();


        }

        // DELETE /items/{id}
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null) return NotFound();

            await itemsRepository.RemoveAsync(item.Id);

            await publishEndpoint.Publish(new CatalogItemDeleted(item.Id));

            return NoContent();
        }

    }
}