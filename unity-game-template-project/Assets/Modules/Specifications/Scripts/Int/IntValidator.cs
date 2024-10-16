namespace Modules.Specifications
{
    public sealed class IntValidator
    {
        private IntGreatOrEqualZeroSpecification _intGreatOrEqualZeroSpecification = new();
        private IntGreatOrEqualOneSpecification _intGreatOrEqualOneSpecification = new();
        private IntBetweenZeroAndOneSpecification _intBetweenZeroAndOneSpecification = new();

        public void GreatOrEqualZero(int value)
        {
            if (_intGreatOrEqualZeroSpecification.IsSatisfiedBy(value) == false)
                throw new System.Exception("The value cannot be less than 0");
        }

        public void GreatOrEqualOne(int value)
        {
            if (_intGreatOrEqualOneSpecification.IsSatisfiedBy(value) == false)
                throw new System.Exception("The value cannot be less than 1");
        }

        public void BetweenZeroAndOne(int value)
        {
            if (_intBetweenZeroAndOneSpecification.IsSatisfiedBy(value) == false)
                throw new System.Exception("The value must be between 0 and 1");
        }
    }
}