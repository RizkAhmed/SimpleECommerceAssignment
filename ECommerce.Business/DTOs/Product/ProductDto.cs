using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Data.Entities;

namespace ECommerce.Business.DTOs.Product
{
    public record ProductDto
    {
        public int Id { get; set; }
        public int Category { get; set; }
        public string ProductCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int MinimumQuantity { get; set; }
        public double DiscountRate { get; set; }
        public static ProductDto Map(Data.Entities.Product p) => new()
        {
            Id = p.Id,
            Category = p.Category,
            ProductCode = p.ProductCode,
            Name = p.Name,
            ImagePath = p.ImagePath,
            Price = p.Price,
            MinimumQuantity = p.MinimumQuantity,
            DiscountRate = p.DiscountRate
        };
    }

}
