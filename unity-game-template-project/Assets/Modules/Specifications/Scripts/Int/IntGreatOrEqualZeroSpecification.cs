namespace Modules.Specifications
{
    public class IntGreatOrEqualZeroSpecification : ISpecification<int>
    {
        public bool IsSatisfiedBy(int item)
        {
            return item >= 0;
        }
    }
}
