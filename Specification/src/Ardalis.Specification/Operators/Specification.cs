namespace Ardalis.Specification.Operators
{
    public abstract class Specification<T> : Ardalis.Specification.Specification<T> where T: class 
    {
        public static bool operator true(Specification<T> specification) => false;
        public static bool operator false(Specification<T> specification) => false;
        public static Specification<T> operator &(Specification<T> lSpec,Specification<T> rSpec) => new AndSpecification<T>(lSpec, rSpec);
        public static Specification<T> operator |(Specification<T> lSpec,Specification<T> rSpec) => new OrSpecification<T>(lSpec, rSpec); 
        public static Specification<T> operator !(Specification<T> specification) => new NotSpecification<T>(specification);
        public static Specification<T> operator -(Specification<T> specification) => new ReverseOrderSpecification<T>(specification);

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return GetType() == other.GetType();
        }
        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
    }
}
