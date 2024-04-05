namespace Ordering.Application.Data;

// IoC 를 위해 Application 에서 생성
public interface IApplicationDbContext
{
  DbSet<Customer> Customers { get; }
  DbSet<Product> Products { get; }
  DbSet<Order> Orders { get; }
  DbSet<OrderItem> OrderItems { get; }

  Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}