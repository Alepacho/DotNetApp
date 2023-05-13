using Microsoft.EntityFrameworkCore;
using WEBAPIapp.Models;

// https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
// https://eduardopereirasousa.tech/?p=91
// https://jasonwatmore.com/post/2022/09/05/net-6-connect-to-sqlite-database-with-entity-framework-core
public class OrderContext : DbContext
{
    private static readonly string[] Titles = new[]
    {
        "Суп", "Суши", "Роллы", "Вок", "Салат", "Бургер", "Сашими"
    };
    
    public OrderContext(
        DbContextOptions<OrderContext> options
    ) : base(options)
    {
        var created = this.Database.EnsureCreated();
        if (created) {
            var orders = (Enumerable.Range(1, 5).Select(index => new Order
            {
                Id    = index,
                Date  = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Title = Titles[Random.Shared.Next(Titles.Length)],
                Price = Random.Shared.Next(99, 1999)
            }).ToArray());

            this.Orders.AddRange(orders);
            this.SaveChanges();
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Filename=test.db");

    public DbSet<Order> Orders { get; set; } = null!;
}