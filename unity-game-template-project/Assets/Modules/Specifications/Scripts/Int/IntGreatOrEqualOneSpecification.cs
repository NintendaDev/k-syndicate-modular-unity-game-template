namespace Modules.Specifications
{
    public sealed class IntGreatOrEqualOneSpecification : ISpecification<int>
    {
        public bool IsSatisfiedBy(int item)
        {
            return item >= 1;
        }
    }
}
