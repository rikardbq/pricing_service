namespace PriceCalculatorApi.Models;

public class CustomServicePrice
{
  public long Id { get; set; }
  public long Price { get; set; }
  public int[] DayRange { get; set; }
  public long ServicePlanId { get; set; }
}
