using Ardalis.Sample.App3;
using Ardalis.Sample.Domain;
using Ardalis.Sample.Domain.Filters;
using Ardalis.Sample.Domain.Specs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

// Sample Application 2
// The focus of this library are not the repositories. We're providing the built-in repository implementations just as a convenience.
// You can certainly have your own repository implementations and tweak them per your needs.
// In this sample we demonstrate how to do that, and some other more advanced features.
// - Defined custom RepositoryBase implementation (also added an additional evaluator)
// - Defined separate IRepository and IReadRepository interfaces. IRepository requires IAggregateRoot.
// - Defined pagination constructs.
// - We're utilizing Automapper projections in our repository and defined ProjectTo methods for the IReadRepository. These methods return paginated response automatically.

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


// Projecting directly to response DTOs. In addition the response is paginated and wrapped in PagedResponse<T>.
app.MapGet("/customers", async (IReadRepository<Customer> repo, [AsParameters] CustomerFilter filter, CancellationToken cancellationToken) =>
{
    var spec = new CustomerSpec(filter);
    var result = await repo.ProjectToListAsync<CustomerDto>(spec, filter, cancellationToken);
    return Results.Ok(result);
});

// Projecting directly to response DTOs.
app.MapGet("/customers/{id}", async (IReadRepository<Customer> repo, int id, CancellationToken cancellationToken) =>
{
    var spec = new CustomerByIdSpec(id);
    var result = await repo.ProjectToFirstOrDefaultAsync<CustomerDto>(spec, cancellationToken);
    if (result is null) return Results.NotFound();
    return Results.Ok(result);
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

public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
{
    public Repository(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public class ReadRepository<T> : RepositoryBase<T>, IReadRepository<T> where T : class
{
    public ReadRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
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
