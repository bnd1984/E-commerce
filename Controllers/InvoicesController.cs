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
    public class InvoicesController : ControllerBase
    {
        // Dependency injection of the IInvoiceService
        private readonly IInvoiceService _invoiceService;

        // Constructor to initialize the IInvoiceService
        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET api/invoices
        // Retrieves all invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetAllInvoices()
        {
            // Calling the service to get all invoices
            var invoices = await _invoiceService.GetAllInvoices();
            return Ok(invoices); // Returning the invoices with an OK status
        }

        // GET api/invoices/{id}
        // Retrieves an invoice by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoiceById(int id)
        {
            // Calling the service to get an invoice by its ID
            var invoice = await _invoiceService.GetInvoiceById(id);
            if (invoice == null)
            {
                // Returning a NotFound status if the invoice is not found
                return NotFound(new { Message = "Invoice not found" });
            }
            return Ok(invoice); // Returning the found invoice with an OK status
        }

        // POST api/invoices
        // Adds a new invoice
        [HttpPost]
        public async Task<ActionResult> AddInvoice([FromBody] Invoice invoice)
        {
            // Validating the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returning a BadRequest status if the model is invalid
            }

            // Calling the service to add the invoice
            await _invoiceService.AddInvoice(invoice);
            // Returning a CreatedAtAction status with the newly created invoice's details
            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
        }

        // PUT api/invoices/{id}
        // Updates an existing invoice
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInvoice(int id, [FromBody] Invoice invoice)
        {
            // Checking if the ID in the URL matches the ID in the request body
            if (id != invoice.Id)
            {
                return BadRequest(new { Message = "Invoice ID mismatch" });
            }

            // Validating the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returning a BadRequest status if the model is invalid
            }

            // Checking if the invoice exists
            var existingInvoice = await _invoiceService.GetInvoiceById(id);
            if (existingInvoice == null)
            {
                return NotFound(new { Message = "Invoice not found" }); // Returning a NotFound status if the invoice is not found
            }

            // Calling the service to update the invoice
            await _invoiceService.UpdateInvoice(invoice);
            return NoContent(); // Returning a NoContent status after successful update
        }

        // DELETE api/invoices/{id}
        // Deletes an existing invoice
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvoice(int id)
        {
            // Calling the service to get an invoice by its ID
            var invoice = await _invoiceService.GetInvoiceById(id);
            if (invoice == null)
            {
                return NotFound(new { Message = "Invoice not found" }); // Returning a NotFound status if the invoice is not found
            }

            // Calling the service to delete the invoice
            await _invoiceService.DeleteInvoice(id);
            return NoContent(); // Returning a NoContent status after successful deletion
        }
    }
}
