using ECommerce.Business.DTOs.Product;
using ECommerce.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService) => _productService = productService;


        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _productService.GetAllAsync());


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await _productService.GetByIdAsync(id));


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ValidationProblem(ModelState);

                var res = await _productService.CreateAsync(request);
                return CreatedAtAction(nameof(Get), new { id = res.Id }, res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ValidationProblem(ModelState);

                return Ok(await _productService.UpdateAsync(id, request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}
