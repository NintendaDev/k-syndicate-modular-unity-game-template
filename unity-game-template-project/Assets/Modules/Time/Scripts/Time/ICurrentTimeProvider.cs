using System;

namespace Modules.Timers.Modules.Time.Scripts.Time
{
    public interface ICurrentTimeProvider
    {
        public DateTime GetCurrentTime();

        public long GetCurrentUnixTimeSeconds();
    }
}