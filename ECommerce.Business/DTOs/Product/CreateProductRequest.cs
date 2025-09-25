using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Business.DTOs.Product
{
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "Please select a category.")]
        public int Category { get; set; }

        [Required(ErrorMessage = "Please enter a product code.")]
        public string ProductCode { get; set; } = null!;

        [Required(ErrorMessage = "Please enter the product name.")]
        public string Name { get; set; } = null!;

        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Please enter the product price.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please enter the minimum quantity.")]
        public int MinimumQuantity { get; set; }

        [Required(ErrorMessage = "Please enter the discount rate.")]
        public double DiscountRate { get; set; }
    }
}
