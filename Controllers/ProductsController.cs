using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoicingSystem.Models;
using InvoicingSystem.Services;

namespace InvoicingSystem.Controllers
{
    // Marking the class as an API controller and setting the route for the controller
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // Dependency injection of the IProductService
        private readonly IProductService _productService;

        // Constructor to initialize the IProductService
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET api/products
        // Retrieves all products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            // Calling the service to get all products
            var products = await _productService.GetAllProducts();
            return Ok(products); // Returning the products with an OK status
        }

        // GET api/products/{id}
        // Retrieves a product by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            // Calling the service to get a product by its ID
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                // Returning a NotFound status if the product is not found
                return NotFound(new { Message = "Product not found" });
            }
            return Ok(product); // Returning the found product with an OK status
        }

        // POST api/products
        // Adds a new product
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] Product product)
        {
            // Validating the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returning a BadRequest status if the model is invalid
            }

            // Calling the service to add the product
            await _productService.AddProduct(product);
            // Returning a CreatedAtAction status with the newly created product's details
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        // PUT api/products/{id}
        // Updates an existing product
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            // Checking if the ID in the URL matches the ID in the request body
            if (id != product.Id)
            {
                return BadRequest(new { Message = "Product ID mismatch" });
            }

            // Validating the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returning a BadRequest status if the model is invalid
            }

            // Checking if the product exists
            var existingProduct = await _productService.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound(new { Message = "Product not found" }); // Returning a NotFound status if the product is not found
            }

            // Calling the service to update the product
            await _productService.UpdateProduct(product);
            return NoContent(); // Returning a NoContent status after successful update
        }

        // DELETE api/products/{id}
        // Deletes an existing product
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            // Calling the service to get a product by its ID
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound(new { Message = "Product not found" }); // Returning a NotFound status if the product is not found
            }

            // Calling the service to delete the product
            await _productService.DeleteProduct(id);
            return NoContent(); // Returning a NoContent status after successful deletion
        }
    }
}
