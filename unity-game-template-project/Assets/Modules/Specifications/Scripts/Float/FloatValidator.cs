namespace Modules.Specifications
{
    public sealed class FloatValidator
    {
        private FloatGreatOrEqualZeroSpecification _floatGreatOrEqualZeroSpecification = new();
        private FloatBetweenZeroAndOneSpecification _floatBetweenZeroAndOneSpecification = new();

        public void GreatOrEqualZero(float value)
        {
            if (_floatGreatOrEqualZeroSpecification.IsSatisfiedBy(value) == false)
                throw new System.Exception("The value cannot be less than 0");
        }

        public void BetweenZeroAndOne(float value)
        {
            if (_floatBetweenZeroAndOneSpecification.IsSatisfiedBy(value) == false)
                throw new System.Exception("The value cannot be greater 1 or less 0");
        }
    }
}