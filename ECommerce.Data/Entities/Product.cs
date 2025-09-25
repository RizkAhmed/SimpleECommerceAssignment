using System.ComponentModel.DataAnnotations;

namespace ECommerce.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        public string ProductCode { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public string? ImagePath { get; set; } 

        public decimal Price { get; set; }

        public int MinimumQuantity { get; set; }

        public double DiscountRate { get; set; }
    }
}
