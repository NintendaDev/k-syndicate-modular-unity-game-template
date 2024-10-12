namespace Modules.Specifications
{
    public class FloatGreatOrEqualZeroSpecification : ISpecification<float>
    {
        public bool IsSatisfiedBy(float item)
        {
            return item >= 0;
        }
    }
}
