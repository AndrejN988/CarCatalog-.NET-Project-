using Car.Service.Catalog.Dtos;
using Car.Service.Catalog.Models;

namespace Car.Service.Catalog
{

    public static class Extensions
    {
        public static CarItemDto AsDto(this CarItem item)
        {
            return new CarItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }

        


        

    }

}