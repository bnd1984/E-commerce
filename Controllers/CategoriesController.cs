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
    public class CategoriesController : ControllerBase
    {
        // Dependency injection of the ICategoryService
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET api/categories
        // Retrieves all categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            // Calling the service to get all categories
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories); // Returning the categories with an OK status
        }

        // GET api/categories/{id}
        // Retrieves a category by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            // Calling the service to get a category by its ID
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                // Returning a NotFound status if the category is not found
                return NotFound(new { Message = "Category not found" });
            }
            return Ok(category); // Returning the found category with an OK status
        }

        // POST api/categories
        // Adds a new category
        [HttpPost]
        public async Task<ActionResult> AddCategory([FromBody] Category category)
        {
            // Validating the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returning a BadRequest status if the model is invalid
            }

            // Calling the service to add the category
            await _categoryService.AddCategory(category);
            // Returning a CreatedAtAction status with the newly created category's details
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        // PUT api/categories/{id}
        // Updates an existing category
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            // Checking if the ID in the URL matches the ID in the request body
            if (id != category.Id)
            {
                return BadRequest(new { Message = "Category ID mismatch" });
            }

            // Validating the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returning a BadRequest status if the model is invalid
            }

            // Checking if the category exists
            var existingCategory = await _categoryService.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound(new { Message = "Category not found" }); // Returning a NotFound status if the category is not found
            }

            // Calling the service to update the category
            await _categoryService.UpdateCategory(category);
            return NoContent(); // Returning a NoContent status after successful update
        }

        // DELETE api/categories/{id}
        // Deletes an existing category
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            // Calling the service to get a category by its ID
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound(new { Message = "Category not found" }); // Returning a NotFound status if the category is not found
            }

            // Calling the service to delete the category
            await _categoryService.DeleteCategory(id);
            return NoContent(); // Returning a NoContent status after successful deletion
        }
    }
}
