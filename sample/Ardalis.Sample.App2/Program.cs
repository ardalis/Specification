using Ardalis.Sample.Domain;
using Ardalis.Sample.Domain.Specs;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

// Sample Application 2
// In this sample we demonstrate various features of specifications.
// - We're utilizing the provided built-in repositories (as in Sample.App1), but we're also have defined a separate ReadRepository.
// - Sample how to add a custom evaluator.
// - Demonstrated the use of SingleResultSpecification
// - Sample of utilizing specifications without repositories (directly with dbContext)
// - Sample of specification with selector (primitive type)
// - Sample of specification with selector (DTO)

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
    // If you want to use the SingleOrDefault methods, the specification must inherit from SingleResultSpecification
    var customer = await repo.SingleOrDefaultAsync(spec, cancellationToken);
    if (customer is null) return Results.NotFound();
    var customerDto = mapper.Map<CustomerDto>(customer);
    return Results.Ok(customerDto);
});

// Using the specification directly with the dbContext (no repositories).
app.MapGet("/customers/v2/{id}", async (AppDbContext dbContext, IMapper mapper, int id, CancellationToken cancellationToken) =>
{
    var spec = new CustomerByIdSpec(id);
    var customer = await dbContext.Customers
        .WithSpecification(spec)
        .FirstOrDefaultAsync(cancellationToken);
    if (customer is null) return Results.NotFound();
    var customerDto = mapper.Map<CustomerDto>(customer);
    return Results.Ok(customerDto);
});

// In this version, we're projecting the result to a DTO directly in the specification
app.MapGet("/customers/v3/{id}", async (IReadRepository<Customer> repo, IMapper mapper, int id, CancellationToken cancellationToken) =>
{
    var spec = new CustomerByIdProjectionSpec(id);
    var customerDto = await repo.FirstOrDefaultAsync(spec, cancellationToken);
    if (customerDto is null) return Results.NotFound();
    return Results.Ok(customerDto);
});

// We're selecting only a name from Customer.
app.MapGet("/customer-names/{id}", async (IReadRepository<Customer> repo, IMapper mapper, int id, CancellationToken cancellationToken) =>
{
    var spec = new CustomerNameSpec(id);
    var name = await repo.FirstOrDefaultAsync(spec, cancellationToken);
    if (name is null) return Results.NotFound();
    return Results.Ok(name);
});


await app.InitializeDbAsync();
app.Run();

public record AddressDto(int Id, string Street, int CustomerId);
public record CustomerDto(int Id, string Name, int Age, List<AddressDto> Addresses);

public class CustomerByIdProjectionSpec : Specification<Customer, CustomerDto>
{
    public CustomerByIdProjectionSpec(int id)
    {
        Query.Where(x => x.Id == id);
        Query.Select(x => new CustomerDto(x.Id, x.Name, x.Age,
            x.Addresses
            .Select(a => new AddressDto(a.Id, a.Street, a.CustomerId))
            .ToList()));
    }
}

public class CustomerNameSpec : Specification<Customer, string>
{
    public CustomerNameSpec(int id)
    {
        Query.Where(x => x.Id == id);
        Query.Select(x => x.Name);
    }
}

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
    // Here we're passing our custom evaluator to the base class.
    public ReadRepository(AppDbContext dbContext) : base(dbContext, AppSpecificationEvaluator.Instance)
    {
    }
}

// We have added a new feature "QueryTag" to the specification. Check the extension in Ardalis.Sample.Domain.CustomerSpecExtensions.
// Now we have to define a custom evaluator to handle this new feature.
// Then we pass our specification evaluator to the repository.
public class AppSpecificationEvaluator : SpecificationEvaluator
{
    public static AppSpecificationEvaluator Instance { get; } = new AppSpecificationEvaluator();

    public AppSpecificationEvaluator() : base()
    {
        Evaluators.Add(new QueryTagEvaluator());
    }
}
public class QueryTagEvaluator : IEvaluator
{
    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.Items.TryGetValue("TagWith", out var value) && value is string tag)
        {
            query = query.TagWith(tag);
        }

        return query;
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
