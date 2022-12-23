using System;
using System.ComponentModel.DataAnnotations;

namespace Car.Service.Catalog.Dtos
{

    public record CarItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);

    public record CreateCarItemDto([Required] string Name, string Description, [Range(25000, 170000)] decimal Price);

    public record UpdateCarItemDto([Required] string Name, string Description, [Range(25000, 170000)] decimal Price);




}