namespace PriceCalculatorApi.Models;

public class ServicePlan
{
  public long Id { get; set; }
  public DateTime Start { get; set; }
  public long CustomerId { get; set; }
  public long ServiceId { get; set; }
  public CustomServicePrice CustomServicePrice { get; set; }
  public Service Service { get; set; }
  public Discount Discount { get; set; }
}
