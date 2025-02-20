using Ardalis.Sample.Domain;
using Ardalis.Sample.Domain.Specs;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

// Sample Application 1
// This application demonstrates the most basic usage of specifications.
// We're utilizing the provided built-in repositories in this sample
// - Define your IRepository interface and inherit from IRepositoryBase
// - Define your Repository and inherit from RepositoryBase. Pass your concrete DbContext to the base class.
// - Register the interface in DI
// You're good to go!

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();


app.MapGet("/customers", async (IRepository<Customer> repo,
                                IMapper mapper,
                                CancellationToken cancellationToken) =>
{
    var spec = new CustomerSpec();
    var customers = await repo.ListAsync(spec, cancellationToken);
    var customersDto = mapper.Map<List<CustomerDto>>(customers);
    return Results.Ok(customersDto);
});

app.MapGet("/customers/{id}", async (IRepository<Customer> repo,
                                     IMapper mapper,
                                     int id,
                                     CancellationToken cancellationToken) =>
{
    var spec = new CustomerByIdSpec(id);
    var customer = await repo.FirstOrDefaultAsync(spec, cancellationToken);
    if (customer is null) return Results.NotFound();
    var customerDto = mapper.Map<CustomerDto>(customer);
    return Results.Ok(customerDto);
});

app.MapPost("/customers", async (IRepository<Customer> repo,
                                 IMapper mapper,
                                 CustomerCreateDto customerCreateDto,
                                 CancellationToken cancellationToken) =>
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

app.MapPut("/customers/{id}", async (IRepository<Customer> repo,
                                     IMapper mapper,
                                     int id,
                                     CustomerUpdateDto customerUpdate,
                                     CancellationToken cancellationToken) =>
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

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}
public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public Repository(AppDbContext dbContext) : base(dbContext)
    {
    }
}

// AutoMapper configuration
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Address, AddressDto>();
        CreateMap<Customer, CustomerDto>();
    }
}
