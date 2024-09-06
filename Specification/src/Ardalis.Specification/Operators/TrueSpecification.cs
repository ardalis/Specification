namespace Ardalis.Specification.Operators
{
    public sealed class TrueSpecification<T> : Specification<T>
    {
        public TrueSpecification()
        {
            Query.Where(e => true);
        }   
    }
}
