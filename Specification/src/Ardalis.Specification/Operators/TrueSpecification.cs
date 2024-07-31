namespace Ardalis.Specification.Operators
{
    public sealed class TrueSpecification<T> : Specification<T> where T:class
    {
        public TrueSpecification()
        {
            Query.Where(e => true);
        }   
    }
}
