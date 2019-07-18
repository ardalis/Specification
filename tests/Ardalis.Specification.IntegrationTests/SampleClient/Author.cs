using System.Collections.Generic;

namespace Ardalis.Specification.IntegrationTests.SampleClient
{
    public class Author : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
