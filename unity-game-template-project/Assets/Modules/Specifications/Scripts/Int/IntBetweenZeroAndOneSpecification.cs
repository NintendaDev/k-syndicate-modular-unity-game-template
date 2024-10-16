namespace Modules.Specifications
{
    public sealed class IntBetweenZeroAndOneSpecification : ISpecification<int>
    {
        public bool IsSatisfiedBy(int item)
        {
            return item >= 0 && item <= 1;
        }
    }
}
