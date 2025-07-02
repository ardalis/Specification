namespace Tests.Fixture;

public class Repository<T>(TestDbContext context) : RepositoryBase<T>(context) where T : class
{
}
