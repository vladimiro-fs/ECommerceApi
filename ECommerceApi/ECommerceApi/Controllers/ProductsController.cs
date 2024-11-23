namespace ECommerceApi.Controllers
{
    using ECommerceApi.Entities;
    using ECommerceApi.Repositories;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string productType, int? categoryId = null)
        {
            IEnumerable<Product> products;

            if (productType == "category" && categoryId != null)
            {
                products = await _productRepository.GetProductsByCategoryAsync(categoryId.Value);
            }
            else if (productType == "popular")
            {
                products = await _productRepository.GetPopularProductsAsync();
            }
            else if (productType == "best seller")
            {
                products = await _productRepository.GetBestSellerProductsAsync();
            }
            else
            {
                return BadRequest("Invalid product type");
            }

            var productData = products.Select(v => new
            {
                Id = v.Id,
                Name = v.Name,
                Price = v.Price,
                ImageUrl = v.ImageUrl,

            });

            return Ok(productData);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductDetails(int id) 
        { 
            var product = await _productRepository.GetProductDetailsAsync(id);

            if (product == null) 
            { 
                return NotFound($"The product with id={id} could not be found");
            }

            var productData = new
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Details = product.Details,
                ImageUrl = product.ImageUrl,
            };

            return Ok(productData);
        }
    }
}
