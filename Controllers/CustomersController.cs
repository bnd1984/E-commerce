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
    public class CustomersController : ControllerBase
    {
        // Dependency injection of the ICustomerService
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET api/customers
        // Retrieves all customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            // Calling the service to get all customers
            var customers = await _customerService.GetAllCustomers();
            return Ok(customers); // Returning the customers with an OK status
        }

        // GET api/customers/{id}
        // Retrieves a customer by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {
            // Calling the service to get a customer by its ID
            var customer = await _customerService.GetCustomerById(id);
            if (customer == null)
            {
                // Returning a NotFound status if the customer is not found
                return NotFound(new { Message = "Customer not found" });
            }
            return Ok(customer); // Returning the found customer with an OK status
        }

        // POST api/customers
        // Adds a new customer
        [HttpPost]
        public async Task<ActionResult> AddCustomer([FromBody] Customer customer)
        {
            // Validating the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returning a BadRequest status if the model is invalid
            }

            // Calling the service to add the customer
            await _customerService.AddCustomer(customer);
            // Returning a CreatedAtAction status with the newly created customer's details
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }

        // PUT api/customers/{id}
        // Updates an existing customer
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            // Checking if the ID in the URL matches the ID in the request body
            if (id != customer.Id)
            {
                return BadRequest(new { Message = "Customer ID mismatch" });
            }

            // Validating the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returning a BadRequest status if the model is invalid
            }

            // Checking if the customer exists
            var existingCustomer = await _customerService.GetCustomerById(id);
            if (existingCustomer == null)
            {
                return NotFound(new { Message = "Customer not found" }); // Returning a NotFound status if the customer is not found
            }

            // Calling the service to update the customer
            await _customerService.UpdateCustomer(customer);
            return NoContent(); // Returning a NoContent status after successful update
        }

        // DELETE api/customers/{id}
        // Deletes an existing customer
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            // Calling the service to get a customer by its ID
            var customer = await _customerService.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound(new { Message = "Customer not found" }); // Returning a NotFound status if the customer is not found
            }

            // Calling the service to delete the customer
            await _customerService.DeleteCustomer(id);
            return NoContent(); // Returning a NoContent status after successful deletion
        }
    }
}
