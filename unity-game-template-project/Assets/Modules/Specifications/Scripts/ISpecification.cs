namespace Modules.Specifications
{
    public interface ISpecification<T>
    {
        public bool IsSatisfiedBy(T item);
    }
}
