namespace PriceCalculatorApi.Models;

public class Customer
{
  public long Id { get; set; }
  public int FreeDays { get; set; }
  public List<ServicePlan> ServicePlans { get; }
}
