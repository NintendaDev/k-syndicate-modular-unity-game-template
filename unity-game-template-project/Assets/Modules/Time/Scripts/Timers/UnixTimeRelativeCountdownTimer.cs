using System;
using Modules.Timers.Modules.Time.Scripts.Time;
using R3;
using UnityEngine;

namespace Modules.TimeUtilities.Timers
{
    public sealed class UnixTimeRelativeCountdownTimer : IDisposable, ICountdownTimer
    {
        private readonly ICurrentTimeProvider _currentTimeProvider;
        private readonly CountdownTimer _timer;

        public UnixTimeRelativeCountdownTimer(ICurrentTimeProvider currentTimeProvider)
        {
            _currentTimeProvider = currentTimeProvider;
            _timer = new CountdownTimer();

            _timer.Finished += OnFinish;
            _timer.Started += OnStart;
        }

        public event Action Finished;
        
        public event Action<float> Started;
        
        public ReadOnlyReactiveProperty<float> Countdown => _timer.Countdown;
        
        public bool IsRunning => _timer.IsRunning;

        public long InitialUnixTime { get; private set; }

        public void Dispose()
        {
            _timer.Finished -= OnFinish;
            _timer.Started -= OnStart;
            _timer.Dispose();
        }

        public void Start(float duration, long initialUnixTime)
        {
            InitialUnixTime = initialUnixTime;
            long nowUnixTime = _currentTimeProvider.GetCurrentUnixTimeSeconds();
            float calculatedDuration = Mathf.Max(initialUnixTime + duration - nowUnixTime, 0);
            
            _timer.Start(calculatedDuration);
        }

        public void Start(float duration)
        {
            InitialUnixTime = _currentTimeProvider.GetCurrentUnixTimeSeconds();
            _timer.Start(duration);
        }

        public void Stop() => _timer.Stop();
        
        private void OnFinish() => Finished?.Invoke();
        
        private void OnStart(float value) => Started?.Invoke(value);
    }
}