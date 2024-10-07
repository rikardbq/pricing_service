namespace PriceCalculatorApi.Models;

public class CalculatePriceRequest
{
  public long Id { get; set; }
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
}
