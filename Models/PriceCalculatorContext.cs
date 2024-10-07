using Microsoft.EntityFrameworkCore;

namespace PriceCalculatorApi.Models;

public class PriceCalculatorContext : DbContext
{
  public PriceCalculatorContext(DbContextOptions<PriceCalculatorContext> options)
      : base(options)
  {
  }

  // protected override void OnModelCreating(ModelBuilder modelBuilder)
  // {
  //   modelBuilder.Entity<TestCase>()
  //       .HasMany(e => e.NestedCases)
  //       .WithOne(e => e.TestCase)
  //       .HasForeignKey(e => e.TestCaseId)
  //       .HasPrincipalKey(e => e.Id);
  // }

  public DbSet<Customer> Customers { get; set; } = null!;
  public DbSet<Service> Services { get; set; } = null!;
  public DbSet<Discount> Discounts { get; set; } = null!;
  public DbSet<ServicePlan> ServicePlans { get; set; } = null!;
}
