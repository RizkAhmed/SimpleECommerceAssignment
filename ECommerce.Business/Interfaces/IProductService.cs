using ECommerce.Business.DTOs.Product;

namespace ECommerce.Business.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateAsync(CreateProductRequest request);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> UpdateAsync(int id, UpdateProductRequest request);
        Task DeleteAsync(int id);
    }
}
