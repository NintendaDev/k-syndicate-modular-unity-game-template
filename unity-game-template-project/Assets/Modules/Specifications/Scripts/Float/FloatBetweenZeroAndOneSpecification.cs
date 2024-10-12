namespace Modules.Specifications
{
    public class FloatBetweenZeroAndOneSpecification : ISpecification<float>
    {
        public bool IsSatisfiedBy(float item)
        {
            return item >= 0 && item <= 1;
        }
    }
}
