using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using System.Reflection;

namespace Ordering.Infrastructure.Data;

// Migration 생성 시
// Ordering.API 프로젝트에 Design 패키지 설치 되어 있어야 하고,
// Ordering.Infrastructure 위치에서 아래와 같이 실행
// ef-command: dotnet ef migrations add InitialCreate -o Data\Migrations -s ..\Ordering.API\Ordering.API.csproj
// 혹은
// 패키지 관리자 콘솔에서는 시작 프로젝트를 Infrastructure 로 지정하고 아래로 실행
// ps-command: Add-Migration InitialCreate -OutputDir Data/Migrations -Project Ordering.Infrastructure -StartupProject Ordering.API
public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  {
  }

  public DbSet<Customer> Customers => Set<Customer>();
  public DbSet<Product> Products => Set<Product>();
  public DbSet<Order> Orders => Set<Order>();
  public DbSet<OrderItem> OrderItems => Set<OrderItem>();

  protected override void OnModelCreating(ModelBuilder builer)
  {
    // too many
    //builer.Entity<Customer>().Property(c => c.Name).IsRequired().HasMaxLength(100);
    // so
    builer.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    base.OnModelCreating(builer);
  }
}