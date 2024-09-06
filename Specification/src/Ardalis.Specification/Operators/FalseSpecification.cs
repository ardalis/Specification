namespace Ardalis.Specification.Operators
{
    public sealed class FalseSpecification<T> : Specification<T>
    {
        public FalseSpecification()
        {
            Query.Where(e => false);
        }   
    }
}
