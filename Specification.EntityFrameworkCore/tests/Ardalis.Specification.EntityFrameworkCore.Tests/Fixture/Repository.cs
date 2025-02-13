namespace Tests.Fixture;

public class Repository<T>(DbContext context) : RepositoryBase<T>(context) where T : class
{
}
