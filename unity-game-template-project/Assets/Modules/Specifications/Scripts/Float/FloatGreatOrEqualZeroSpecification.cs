namespace Modules.Specifications
{
    public sealed class FloatGreatOrEqualZeroSpecification : ISpecification<float>
    {
        public bool IsSatisfiedBy(float item)
        {
            return item >= 0;
        }
    }
}
