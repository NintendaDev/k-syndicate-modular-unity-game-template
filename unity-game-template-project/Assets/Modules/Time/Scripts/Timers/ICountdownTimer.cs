using System;
using R3;

namespace Modules.TimeUtilities.Timers
{
    public interface ICountdownTimer
    {
        public event Action Finished;
        
        public event Action<float> Started;
        
        public ReadOnlyReactiveProperty<float> Countdown { get; }

        public bool IsRunning { get; }

        public void Start(float duration);

        public void Stop();
    }
}