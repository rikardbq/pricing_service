namespace PriceCalculatorApi.Models;

public class Discount
{
  public long Id { get; set; }
  public int Amount { get; set; }
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
  public long ServiceId { get; set; }
}
