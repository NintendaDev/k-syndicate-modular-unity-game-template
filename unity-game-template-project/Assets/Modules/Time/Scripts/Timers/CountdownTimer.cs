using System;
using R3;
using UnityEngine;

namespace Modules.TimeUtilities.Timers
{
    public sealed class CountdownTimer : IDisposable, ICountdownTimer
    {
        private CompositeDisposable _disposables = new();
        private ReactiveProperty<float> _countdown = new();
        
        public event Action Finished;
                        
        public event Action<float> Started;

        public ReadOnlyReactiveProperty<float> Countdown => _countdown;

        public bool IsRunning => _countdown.Value > 0;

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Start(float duration)
        {
            if (duration < 0)
                throw new ArgumentOutOfRangeException(nameof(duration));
            
            if (duration == 0)
                return;

            Stop();

            _countdown.Value = duration;
            Started?.Invoke(_countdown.Value);
            
            Observable.EveryUpdate()
                .Subscribe((_) => Tick())
                .AddTo(_disposables);
        }

        public void Stop()
        {
            _disposables.Clear();
            _countdown.Value = 0;
        }

        private void Tick()
        {
            _countdown.Value = Mathf.Max(_countdown.Value - Time.deltaTime, 0);

            if (_countdown.Value != 0)
                return;
            
            Stop();
            Finished?.Invoke();
        }
    }
}