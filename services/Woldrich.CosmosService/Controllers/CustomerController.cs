using Microsoft.AspNetCore.Mvc;
using Woldrich.CosmosData;
using Woldrich.CosmosModel;

namespace Woldrich.CosmosService.Controllers;

[ApiController]
[Route("/woldrich/customer")]
public class CustomerController : ControllerBase
{
    CustomerDao CustomerDao = new CustomerDao();

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Customer>> CreateCustomerAsync(Customer customer) {
        // TODO: should we assert that HashKey and RangeKey are null and we're being asked to create a new Customer in Cosmos db
        customer.GenerateRandomKeys();

        await CustomerDao.CrupdateCustomer(customer);

        return CreatedAtAction(nameof(GetCustomerAsync), new { hashKey = customer.HashKey, rangeKey = customer.RangeKey }, customer);
    }

    [HttpGet]
    public async Task<ActionResult<Customer>> GetCustomerAsync([FromQuery] CosmosKey id) {
        Customer? customer = await CustomerDao.GetCustomerByIdAsync(id);

        if (customer == null) {
            return NotFound();
        }

        return Ok(customer);
    }
}