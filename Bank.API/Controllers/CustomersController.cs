using System.Collections.Generic;
using Bank.API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bank.API.Controllers
{
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
        public IActionResult Get(int id)
        {
            return Ok(ctx.Customers.Find(id));
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]Customer customer)
        {
            ctx.Customers.Add(customer);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            ctx.Customers.Remove(ctx.Customers.Find(id));
        }
    }
}
