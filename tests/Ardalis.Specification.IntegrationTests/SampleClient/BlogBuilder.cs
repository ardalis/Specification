namespace Ardalis.Specification.IntegrationTests.SampleClient
{
    public class BlogBuilder
    {
        public const int VALID_BLOG_ID = 1;
        public const string VALID_BLOG_NAME = "Test Blog 1";
        public const string VALID_BLOG_URL = "ardalis.com";

        private readonly Blog _blog = new Blog();

        public BlogBuilder Id(int id)
        {
            _blog.Id = id;
            return this;
        }

        public BlogBuilder Name(string name)
        {
            _blog.Name = name;
            return this;
        }

        public BlogBuilder Url(string url)
        {
            _blog.Url = url;
            return this;
        }

        public Blog Build()
        {
            return _blog;
        }

        public BlogBuilder WithTestValues()
        {
            _blog.Id = VALID_BLOG_ID;
            _blog.Name = VALID_BLOG_NAME;
            _blog.Url = VALID_BLOG_URL;

            return this;
        }
    }
}
