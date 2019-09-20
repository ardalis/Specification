using System.Collections.Generic;

namespace Ardalis.Specification.IntegrationTests.SampleClient
{
    public class Blog : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
