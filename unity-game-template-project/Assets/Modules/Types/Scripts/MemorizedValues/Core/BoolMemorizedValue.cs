namespace Modules.Types.MemorizedValues.Core
{
    public sealed class BoolMemorizedValue : MemorizedValue<bool>
    {
        public BoolMemorizedValue() : base()
        {
        }

        public BoolMemorizedValue(bool value) : base(value)
        {
        }
    }
}