using ECommerce.Business.DTOs.Product;
using ECommerce.Business.Interfaces;
using ECommerce.Data.Entities;
using ECommerce.Data.Interfaces;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Utilities;

namespace ECommerce.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public ProductService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<ProductDto> CreateAsync(CreateProductRequest request)
        {
            var existing = await _unitOfWork.Products.GetByCodeAsync(request.ProductCode);

            if (existing != null)
                throw new Exception("Product code already exists");

            if (request.ImageFile == null || request.ImageFile.Length == 0)
                throw new Exception("Please upload a valid file");

            var entity = new Product
            {
                Category = request.Category,
                ProductCode = request.ProductCode,
                Name = request.Name,
                Price = request.Price,
                MinimumQuantity = request.MinimumQuantity,
                DiscountRate = request.DiscountRate
            };

            entity.ImagePath = await _fileService.SaveFileAsync(request.ImageFile);

            await _unitOfWork.Products.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();


            return ProductDto.Map(entity);
        }


        public async Task DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id) ?? throw new Exception("Not found");
            _unitOfWork.Products.Remove(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var list = await _unitOfWork.Products.GetAllAsync();
            return list.Select(ProductDto.Map);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            return product == null ? null : ProductDto.Map(product);
        }


        public async Task<ProductDto> UpdateAsync(int id, UpdateProductRequest request)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id)
                          ?? throw new Exception("Not found");

            var existing = await _unitOfWork.Products.GetByCodeAsync(request.ProductCode);

            if (existing != null && existing.Id != id)
                throw new Exception("Product code already exists");

            product.Category = request.Category;
            product.Name = request.Name;
            product.Price = request.Price;
            product.MinimumQuantity = request.MinimumQuantity;
            product.DiscountRate = request.DiscountRate;

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                // Delete the old file
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    _fileService.DeleteFile(product.ImagePath);
                }
                // Save new file
                product.ImagePath = await _fileService.SaveFileAsync(request.ImageFile);
            }

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return ProductDto.Map(product);
        }

    }
}
