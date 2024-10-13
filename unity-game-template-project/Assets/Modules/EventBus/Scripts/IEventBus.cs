using System;

namespace Modules.EventBus
{
    public interface IEventBus
    {
        public void Subscribe<TSignal>(Action<TSignal> callback) where TSignal : IPayloadSignal;

        public void Subscribe<TSignal>(Action callback) where TSignal : ISimpleSignal;

        public void Unsubscribe<TSignal>(Action<TSignal> callback) where TSignal : IPayloadSignal;

        public void Unsubscribe<TSignal>(Action callback) where TSignal : ISimpleSignal;

        public void Invoke<TSignal>(TSignal signal) where TSignal : IPayloadSignal;

        public void Invoke<TSignal>() where TSignal : ISimpleSignal;
    }
}