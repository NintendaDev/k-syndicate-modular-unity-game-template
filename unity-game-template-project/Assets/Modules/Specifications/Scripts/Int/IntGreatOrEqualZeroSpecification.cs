namespace Modules.Specifications
{
    public sealed class IntGreatOrEqualZeroSpecification : ISpecification<int>
    {
        public bool IsSatisfiedBy(int item)
        {
            return item >= 0;
        }
    }
}
