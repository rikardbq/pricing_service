using Microsoft.AspNetCore.Mvc;
using PriceCalculatorApi.Models;
using PriceCalculatorApi.Repositories;

namespace pricing_service.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PriceCalculatorController : ControllerBase
  {
    private readonly PriceCalculatorRepository _priceCalculatorRepository;

    public PriceCalculatorController(PriceCalculatorRepository priceCalculatorRepository)
    {
      _priceCalculatorRepository = priceCalculatorRepository;
    }

    // GET: api/PriceCalculator/GetCustomers
    [HttpGet("GetCustomers")]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
      return await _priceCalculatorRepository.GetCustomers();
    }

    [HttpGet("GetCustomers/{id}")]
    public async Task<ActionResult<Customer>> GetCustomerFromId(long id)
    {
      var customer = await _priceCalculatorRepository.GetCustomerFromId(id);

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
      var customer = await _priceCalculatorRepository.GetCustomerFromId(calculatePriceRequest.Id);

      if (customer == null)
      {
        return NotFound();
      }

      var price = _priceCalculatorRepository.CalculateTotalPriceForCustomer(
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
      _priceCalculatorRepository.CreateTestData();

      return StatusCode(200);
    }
  }
}
