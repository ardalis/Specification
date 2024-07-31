namespace Ardalis.Specification.Operators
{
    public sealed class FalseSpecification<T> : Specification<T>where T:class
    {
        public FalseSpecification()
        {
            Query.Where(e => false);
        }   
    }
}
