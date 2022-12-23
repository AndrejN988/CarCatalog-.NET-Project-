using System;
using Play.Common;

namespace Car.Service.Catalog.Models
{

    public class CarItem : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTimeOffset CreatedDate { get; set; }



    }
}