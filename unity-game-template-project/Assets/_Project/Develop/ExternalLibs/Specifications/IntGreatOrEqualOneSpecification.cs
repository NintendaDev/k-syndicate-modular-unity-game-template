namespace ExternalLibraries.Specifications
{
    public class IntGreatOrEqualOneSpecification : ISpecification<int>
    {
        public bool IsSatisfiedBy(int item)
        {
            return item >= 1;
        }
    }
}
