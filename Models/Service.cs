namespace PriceCalculatorApi.Models;

public class Service
{
  public long Id { get; set; }
  public string Name { get; set; }
  public long Price { get; set; }
  public int[] DayRange { get; set; }
}
