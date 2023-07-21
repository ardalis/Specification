using Ardalis.Sample.Domain;
using Ardalis.Sample.Domain.Specs;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();


app.MapGet("/customers", async (IReadRepository<Customer> repo, IMapper mapper, CancellationToken cancellationToken) =>
{
    var spec = new CustomerSpec();
    var customers = await repo.ListAsync(spec, cancellationToken);
    var customersDto = mapper.Map<List<CustomerDto>>(customers);
    return Results.Ok(customersDto);
});

app.MapGet("/customers/{id}", async (IReadRepository<Customer> repo, IMapper mapper, int id, CancellationToken cancellationToken) =>
{
    var spec = new CustomerByIdSpec(id);
    var customer = await repo.FirstOrDefaultAsync(spec, cancellationToken);
    if (customer is null) return Results.NotFound();
    var customerDto = mapper.Map<CustomerDto>(customer);
    return Results.Ok(customerDto);
});

app.MapPost("/customers", async (IRepository<Customer> repo, IMapper mapper, CustomerCreateDto customerCreateDto, CancellationToken cancellationToken) =>
{
    var customer = new Customer
    {
        Name = customerCreateDto.Name,
        Age = customerCreateDto.Age,
        Addresses = customerCreateDto.Addresses.Select(a => new Address { Street = a.Street }).ToList()
    };
    await repo.AddAsync(customer, cancellationToken);
    var customerDto = mapper.Map<CustomerDto>(customer);
    return Results.Ok(customerDto);
});

app.MapPut("/customers/{id}", async (IRepository<Customer> repo, IMapper mapper, int id, CustomerUpdateDto customerUpdate, CancellationToken cancellationToken) =>
{
    var spec = new CustomerByIdSpec(id);
    var customer = await repo.FirstOrDefaultAsync(spec, cancellationToken);
    if (customer is null) return Results.NotFound();
    customer.Name = customerUpdate.Name;
    customer.Age = customerUpdate.Age;
    await repo.UpdateAsync(customer, cancellationToken);
    var customerDto = mapper.Map<CustomerDto>(customer);
    return Results.Ok(customerDto);
});

await app.InitializeDbAsync();
app.Run();

public record AddressDto(int Id, string Street, int CustomerId);
public record AddressCreateDto(string Street);
public record CustomerDto(int Id, string Name, int Age, List<AddressDto> Addresses);
public record CustomerCreateDto(string Name, int Age, List<AddressCreateDto> Addresses);
public record CustomerUpdateDto(string Name, int Age);

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}
public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
{
}
public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
{
    public Repository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
public class ReadRepository<T> : RepositoryBase<T>, IReadRepository<T> where T : class
{
    public ReadRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Address, AddressDto>();
        CreateMap<Customer, CustomerDto>();
    }
}

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
