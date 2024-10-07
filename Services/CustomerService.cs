namespace PriceCalculatorApi.Services;

using System;
using System.Linq;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceCalculatorApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

public class CustomerService
{

  private readonly PriceCalculatorContext _context;

  public CustomerService(PriceCalculatorContext context)
  {
    _context = context;
  }

  public async void CreateCustomerData()
  {
    Customer customerX = new Customer
    {
      Id = 1,
      FreeDays = 0
    };
    ServicePlan servicePlanCustomerXServiceA = new ServicePlan
    {
      Id = 999,
      CustomerId = customerX.Id,
      Start = new DateTime(2019, 9, 20)
    };
    ServicePlan servicePlanCustomerXServiceC = new ServicePlan
    {
      Id = 888,
      CustomerId = customerX.Id,
      Start = new DateTime(2019, 9, 20)
    };
    Service serviceA = new Service
    {
      Id = 55555,
      Name = "SERVICE_A",
      DayRange = [1, 5],
      Price = 200,
      ServicePlanId = servicePlanCustomerXServiceA.Id
    };
    Service serviceC = new Service
    {
      Id = 66666,
      Name = "SERVICE_C",
      DayRange = [0, 6],
      Price = 400,
      ServicePlanId = servicePlanCustomerXServiceC.Id
    };
    Discount discountCustomerXServiceC = new Discount
    {
      Id = 123,
      Amount = 20,
      Start = new DateTime(2019, 9, 22),
      End = new DateTime(2019, 9, 24),
      ServiceId = serviceC.Id
    };

    _context.Customers.Add(customerX);
    _context.Services.AddRange([serviceA, serviceC]);
    _context.ServicePlans.AddRange([servicePlanCustomerXServiceA, servicePlanCustomerXServiceC]);
    _context.Discounts.Add(discountCustomerXServiceC);

    await _context.SaveChangesAsync();
  }

  public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
  {
    return await _context.Customers
    .Include(c => c.ServicePlans)
    .ThenInclude(sp => sp.Service)
    .ThenInclude(s => s.Discount)
    .ToListAsync();
  }

  public async Task<ActionResult<Customer>> GetCustomerFromId(long id)
  {
#pragma warning disable CS8604 // Possible null reference argument.
    return await _context.Customers
    .Include(c => c.ServicePlans)
    .ThenInclude(sp => sp.Service)
    .ThenInclude(s => s.Discount)
    .FirstOrDefaultAsync(c => c.Id == id);
#pragma warning restore CS8604 // Possible null reference argument.
  }

  public long CalculateTotalPriceForCustomer(Customer customer, DateTime start, DateTime end, string callingService)
  {
    long totalPrice = 0;
    ServicePlan servicePlan = customer.ServicePlans.Find(sp => sp.Service.Name == callingService);
    Service service = servicePlan.Service;
    Discount discount = service.Discount;
    int daysBetweenDates = end.Subtract(start).Days;
    List<DateTime> chargableDates = new List<DateTime>();

    bool isInsideServiceDayRange(DateTime date)
    {
      int dateDayOfWeek = (int)date.DayOfWeek;

      return dateDayOfWeek >= service.DayRange[0] && dateDayOfWeek <= service.DayRange[1];
    }

    for (int i = 0; i <= daysBetweenDates; i++)
    {
      TimeSpan timeToAdd = new TimeSpan(i, 0, 0, 0);
      DateTime curr = start.Add(timeToAdd);

      if (isInsideServiceDayRange(curr))
      {
        if (discount != null && curr >= discount.Start && curr <= discount.End)
        {
          totalPrice = totalPrice + (service.Price * (discount.Amount / 1000));
        }
        else
        {
          totalPrice = totalPrice + service.Price;
        }

        // chargableDates.Add(curr);
      }
    }

    return totalPrice;
  }
}
