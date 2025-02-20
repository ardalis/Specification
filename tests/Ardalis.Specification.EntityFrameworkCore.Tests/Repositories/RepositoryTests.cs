//using System.Runtime.CompilerServices;

//namespace Tests.Repositories;

//[Collection("SharedCollection")]
//public class RepositoryTests(TestFactory factory) : IntegrationTest(factory)
//{
//    [Fact]
//    public void Constructor_SetsDbContext()
//    {
//        var repo = new Repository<object>(DbContext);

//        Accessors<object>.DbContextOf(repo).Should().BeSameAs(DbContext);
//        Accessors<object>.SpecificationEvaluatorOf(repo).Should().BeSameAs(SpecificationEvaluator.Default);
//        Accessors<object>.PaginationSettingsOf(repo).Should().BeSameAs(PaginationSettings.Default);
//    }

//    [Fact]
//    public void Constructor_SetsDbContextAndEvaluator()
//    {
//        var evaluator = new SpecificationEvaluator();
//        var repo = new Repository<object>(DbContext, evaluator);

//        Accessors<object>.DbContextOf(repo).Should().BeSameAs(DbContext);
//        Accessors<object>.SpecificationEvaluatorOf(repo).Should().BeSameAs(evaluator);
//        Accessors<object>.PaginationSettingsOf(repo).Should().BeSameAs(PaginationSettings.Default);
//    }

//    [Fact]
//    public void Constructor_SetsDbContextAndPaginationSettings()
//    {
//        var paginationSettings = new PaginationSettings(20, 200);
//        var repo = new Repository<object>(DbContext, paginationSettings);

//        Accessors<object>.DbContextOf(repo).Should().BeSameAs(DbContext);
//        Accessors<object>.SpecificationEvaluatorOf(repo).Should().BeSameAs(SpecificationEvaluator.Default);
//        Accessors<object>.PaginationSettingsOf(repo).Should().BeSameAs(paginationSettings);
//    }

//    [Fact]
//    public void Constructor_SetsDbContextAndEvaluatorAndPaginationSettings()
//    {
//        var evaluator = new SpecificationEvaluator();
//        var paginationSettings = new PaginationSettings(20, 200);
//        var repo = new Repository<object>(DbContext, evaluator, paginationSettings);

//        Accessors<object>.DbContextOf(repo).Should().BeSameAs(DbContext);
//        Accessors<object>.SpecificationEvaluatorOf(repo).Should().BeSameAs(evaluator);
//        Accessors<object>.PaginationSettingsOf(repo).Should().BeSameAs(paginationSettings);
//    }

//    public class Repository<T> : RepositoryWithMapper<T> where T : class
//    {
//        public Repository(DbContext context)
//            : base(context)
//        {
//        }

//        public Repository(DbContext dbContext, SpecificationEvaluator specificationEvaluator)
//            : base(dbContext, specificationEvaluator)
//        {
//        }

//        public Repository(DbContext dbContext, PaginationSettings paginationSettings)
//            : base(dbContext, paginationSettings)
//        {
//        }

//        public Repository(DbContext dbContext, SpecificationEvaluator specificationEvaluator, PaginationSettings paginationSettings)
//            : base(dbContext, specificationEvaluator, paginationSettings)
//        {
//        }

//        protected override IQueryable<TResult> Map<TResult>(IQueryable<T> source)
//            => throw new NotImplementedException();
//    }

//    private class Accessors<T> where T : class
//    {
//        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_dbContext")]
//        public static extern ref DbContext DbContextOf(RepositoryBase<T> @this);

//        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_evaluator")]
//        public static extern ref SpecificationEvaluator SpecificationEvaluatorOf(RepositoryBase<T> @this);

//        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_paginationSettings")]
//        public static extern ref PaginationSettings PaginationSettingsOf(RepositoryWithMapper<T> @this);
//    }
//}
