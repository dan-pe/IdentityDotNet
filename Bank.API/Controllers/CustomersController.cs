using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly BankContext ctx;

        public CustomersController(BankContext ctx)
        {
            this.ctx = ctx;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            return ctx.Customers;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult GetCustomer([FromRoute]long id)
        {
            return Ok(ctx.Customers.Find(id));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody]Customer customer)
        {
            await ctx.Customers.AddAsync(customer);
            ctx.SaveChanges();

            return Ok(customer);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute]long id, [FromBody]Customer updatedCustomer)
        {

            if (ctx.Customers.Any(c => c.Id.Equals(id)))
            {
                var customer = ctx.Customers.Find(id);
                customer.FirstName = updatedCustomer.FirstName;
                customer.LastName = updatedCustomer.LastName;
                await ctx.SaveChangesAsync();
            }

            return Ok(updatedCustomer);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void DeleteCustomer([FromRoute]long id)
        {
            ctx.Customers.Remove(ctx.Customers.Find(id));
            ctx.SaveChanges();
        }
    }
}
