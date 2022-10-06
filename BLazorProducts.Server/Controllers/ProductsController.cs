using BLazorProducts.Server.Repository;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BLazorProducts.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProductParameters productParameters) {
            var products = await _repo.GetProducts(productParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(products.MetaData));

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product) {
            if (product == null) {
                return BadRequest();
            }

            await _repo.CreateProduct(product);
            return Created("", product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id) {
            var product = await _repo.GetProduct(id);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody]Product product) { 
            var dbProduct = await _repo.GetProduct(id);
            if (dbProduct == null)
                return NotFound();

            await _repo.UpdateProduct(product, dbProduct);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id) {
            var product = await _repo.GetProduct(id);
            if (product == null)
                return NotFound();

            await _repo.DeleteProduct(product);

            return NoContent();
        }
    }
}
