using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using PriceCalculatorApi.Models;
using PriceCalculatorApi.Services;
using TodoApi.Models;

namespace pricing_service.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PriceCalculatorController : ControllerBase
  {
    private readonly CustomerService _customerService;

    public PriceCalculatorController(CustomerService customerService)
    {
      _customerService = customerService;
    }

    // GET: api/PriceCalculator/GetCustomers
    [HttpGet("GetCustomers")]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
      Console.WriteLine("GET DATA");
      return await _customerService.GetCustomers();
    }

    [HttpGet("GetCustomers/{id}")]
    public async Task<ActionResult<Customer>> GetCustomerFromId(long id)
    {
      var customer = await _customerService.GetCustomerFromId(id);

      if (customer.Value == null)
      {
        return NotFound();
      }

      return customer;
    }

    // POST: api/PriceCalculator/CalculatePrice
    [HttpPost("CalculatePrice")]
    public async Task<ActionResult<long>> CalculatePrice(CalculatePriceRequest calculatePriceRequest)
    {
      string callingServiceName = Request.Headers["caller"].ToString();
      var customer = await _customerService.GetCustomerFromId(calculatePriceRequest.Id);

      if (customer == null)
      {
        return NotFound();
      }

      var price = _customerService.CalculateTotalPriceForCustomer(
        customer.Value,
        calculatePriceRequest.Start,
        calculatePriceRequest.End,
        callingServiceName);

      return price;
    }

    // POST: api/PriceCalculator/CreateTestData
    [HttpPost("CreateTestData")]
    public async Task<ActionResult<int>> CreateTestData()
    {
      Console.WriteLine("CREATE");
      _customerService.CreateCustomerData();

      return StatusCode(200);
    }

    // SAVE check
    private bool TodoItemExists(long id)
    {
      return true; //_context.TodoItems.Any(e => e.Id == id);
    }
  }
}
