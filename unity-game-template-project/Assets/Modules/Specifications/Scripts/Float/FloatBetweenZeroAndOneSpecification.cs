namespace Modules.Specifications
{
    public sealed class FloatBetweenZeroAndOneSpecification : ISpecification<float>
    {
        public bool IsSatisfiedBy(float item)
        {
            return item >= 0 && item <= 1;
        }
    }
}
