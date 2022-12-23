using System;
using Play.Common;

namespace Car.Inventory.Service.Models
{
    public class CatalogItem : IEntity 
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
    }
}