namespace Ardalis.Specification.IntegrationTests.SampleClient
{
    public class Post : IEntity<int>
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int AuthorId { get; set; }

        public string Content { get; set; }
        public string Title { get; set; }
    }
}
