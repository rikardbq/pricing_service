namespace PriceCalculatorApi.Repositories;

using System;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceCalculatorApi.Models;

public class PriceCalculatorRepository
{

  private readonly PriceCalculatorContext _context;

  public PriceCalculatorRepository(PriceCalculatorContext context)
  {
    _context = context;
  }

  public async void CreateTestData()
  {
    // Test case 1 data
    Customer customerX = new Customer
    {
      Id = 1,
      FreeDays = 0,
    };
    ServicePlan customerXServicePlanServiceA = new ServicePlan
    {
      Id = 999,
      CustomerId = customerX.Id,
      Start = new DateTime(2019, 9, 20),
      ServiceId = 55555
    };
    ServicePlan customerXServicePlanServiceC = new ServicePlan
    {
      Id = 888,
      CustomerId = customerX.Id,
      Start = new DateTime(2019, 9, 20),
      ServiceId = 66666
    };
    Service serviceA = new Service
    {
      Id = 55555,
      Name = "SERVICE_A",
      DayRange = [1, 5],
      Price = 200,
    };
    Service serviceC = new Service
    {
      Id = 66666,
      Name = "SERVICE_C",
      DayRange = [0, 6],
      Price = 400,
    };
    Discount customerXDiscountServicePlanServiceC = new Discount
    {
      Id = 123,
      Amount = 20,
      Start = new DateTime(2019, 9, 22),
      End = new DateTime(2019, 9, 24),
      ServicePlanId = customerXServicePlanServiceC.Id,
    };

    // test case 2 data
    Customer customerY = new Customer
    {
      Id = 2,
      FreeDays = 200,
    };
    ServicePlan customerYServicePlanServiceB = new ServicePlan
    {
      Id = 777,
      CustomerId = customerY.Id,
      Start = new DateTime(2018, 1, 1),
      ServiceId = 44444,
    };
    ServicePlan customerYServicePlanServiceC = new ServicePlan
    {
      Id = 666,
      CustomerId = customerY.Id,
      Start = new DateTime(2018, 1, 1),
      ServiceId = 66666,
    };
    Service serviceB = new Service
    {
      Id = 44444,
      Name = "SERVICE_B",
      DayRange = [1, 5],
      Price = 240,
    };
    Discount customerYDiscountServicePlanServiceC = new Discount
    {
      Id = 456,
      Amount = 30,
      Start = new DateTime(2018, 1, 1),
      End = new DateTime(),
      ServicePlanId = customerYServicePlanServiceC.Id,
    };
    Discount customerYDiscountServicePlanServiceB = new Discount
    {
      Id = 789,
      Amount = 30,
      Start = new DateTime(2018, 1, 1),
      End = new DateTime(),
      ServicePlanId = customerYServicePlanServiceB.Id,
    };

    _context.Customers.AddRange([
      customerX,
      customerY
    ]);
    _context.Services.AddRange([
      serviceA,
      serviceB,
      serviceC,
    ]);
    _context.ServicePlans.AddRange([
      customerXServicePlanServiceA,
      customerXServicePlanServiceC,
      customerYServicePlanServiceB,
      customerYServicePlanServiceC
    ]);
    _context.Discounts.AddRange([
      customerXDiscountServicePlanServiceC,
      customerYDiscountServicePlanServiceC,
      customerYDiscountServicePlanServiceB
    ]);

    await _context.SaveChangesAsync();
  }

  public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
  {
    return await _context.Customers
      .Include(c => c.ServicePlans)
        .ThenInclude(sp => sp.Service)
      .Include(c => c.ServicePlans)
        .ThenInclude(sp => sp.Discount)
    .ToListAsync();
  }

  public async Task<ActionResult<Customer>> GetCustomerFromId(long id)
  {
#pragma warning disable CS8604 // Possible null reference argument.
    return await _context.Customers
      .Include(c => c.ServicePlans)
        .ThenInclude(sp => sp.Service)
      .Include(c => c.ServicePlans)
        .ThenInclude(sp => sp.Discount)
      .FirstOrDefaultAsync(c => c.Id == id);
#pragma warning restore CS8604 // Possible null reference argument.
  }

  public long CalculateTotalPriceForCustomer(Customer customer, DateTime start, DateTime end, string callingService)
  {
    long totalPrice = 0;
    ServicePlan servicePlan = customer.ServicePlans.Find(sp => sp.Service.Name == callingService);

    if (servicePlan == null)
    {
      return totalPrice;
    }
    Service service = servicePlan.Service;
    CustomServicePrice customServicePrice = servicePlan.CustomServicePrice;
    Discount discount = servicePlan.Discount;
    int daysBetweenDates = end.Subtract(start).Days;
    int freeDays = customer.FreeDays;
    long servicePrice = service.Price;
    int[] serviceDayRange = service.DayRange;

    if (customServicePrice != null)
    {
      servicePrice = customServicePrice.Price;
      serviceDayRange = customServicePrice.DayRange;
    }

    for (int i = 0; i <= daysBetweenDates; i++)
    {
      TimeSpan timeToAdd = new TimeSpan(i, 0, 0, 0);
      DateTime curr = start.Add(timeToAdd);

      if (isInsideServiceDayRange(curr, serviceDayRange))
      {
        if (freeDays == 0)
        {
          if (discount != null && (discount.End < discount.Start || (curr >= discount.Start && curr <= discount.End)))
          {
            totalPrice = totalPrice + (servicePrice * discount.Amount / 100);
          }
          else
          {
            totalPrice = totalPrice + servicePrice;
          }
        }
        else
        {
          freeDays--;
        }
      }
    }

    return totalPrice;
  }

  private bool isInsideServiceDayRange(DateTime date, int[] range)
  {
    int dateDayOfWeek = (int)date.DayOfWeek;

    return dateDayOfWeek >= range[0] && dateDayOfWeek <= range[1];
  }
}
