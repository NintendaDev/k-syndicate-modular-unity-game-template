namespace Modules.Types.MemorizedValues.Core
{
    public sealed class LongMemorizedValue : MemorizedValue<long>
    {
        public LongMemorizedValue() : base()
        {
        }

        public LongMemorizedValue(long value) : base(value)
        {
        }
    }
}