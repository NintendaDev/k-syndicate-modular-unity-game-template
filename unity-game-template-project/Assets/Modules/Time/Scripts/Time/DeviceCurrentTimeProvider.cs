using System;

namespace Modules.Timers.Modules.Time.Scripts.Time
{
    public sealed class DeviceCurrentTimeProvider : ICurrentTimeProvider
    {
        public DateTime GetCurrentTime() => DateTime.Now;
        
        public long GetCurrentUnixTimeSeconds() => DateTimeOffset.Now.ToUnixTimeSeconds();
    }
}