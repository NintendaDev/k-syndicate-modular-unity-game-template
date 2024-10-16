namespace Modules.Types.MemorizedValues.Core
{
    public sealed class FloatMemorizedValue : MemorizedValue<float>
    {
        public FloatMemorizedValue() : base()
        {
        }

        public FloatMemorizedValue(long value) : base(value)
        {
        }
    }
}