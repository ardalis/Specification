using Ardalis.Sample.Domain;

public static class WebApplicationExtensions
{
    public static async Task InitializeDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
        var customers = new List<Customer>()
        {
            new()
            {
                Name = "Customer1",
                Age = 20,
                Addresses = new()
                {
                    new() { Street = "Street1_1" },
                    new() { Street = "Street1_2" }
                }
            },
            new()
            {
                Name = "Customer2",
                Age = 30,
                Addresses = new()
                {
                    new() { Street = "Street2_1" },
                    new() { Street = "Street3_2" }
                }
            }
        };
        dbContext.Customers.AddRange(customers);
        await dbContext.SaveChangesAsync();
    }
}
