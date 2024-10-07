namespace PriceCalculatorApi.Models;

public class ServicePlan
{
  public long Id { get; set; }
  public DateTime Start { get; set; }
  public long CustomerId { get; set; }
  public Service Service { get; set; }
}
