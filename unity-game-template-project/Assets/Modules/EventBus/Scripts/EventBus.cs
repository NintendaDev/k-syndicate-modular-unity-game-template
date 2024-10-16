using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.EventBus
{
    public sealed class EventBus : IEventBus
    {
        private readonly string _incorrectCallbackTypeMessage = "Incorrect callback type";
        private Dictionary<string, List<object>> _signalCallbacks = new();

        public void Subscribe<TSignal>(Action<TSignal> callback) where TSignal : IPayloadSignal =>
            Subscribe<TSignal>((object)callback);

        public void Subscribe<TSignal>(Action callback) where TSignal : ISimpleSignal =>
            Subscribe<TSignal>((object)callback);

        public void Unsubscribe<TSignal>(Action<TSignal> callback) where TSignal : IPayloadSignal =>
            Unsubscribe<TSignal>((object)callback);

        public void Unsubscribe<TSignal>(Action callback) where TSignal : ISimpleSignal =>
            Unsubscribe<TSignal>((object)callback);

        public void Invoke<TSignal>(TSignal signal) where TSignal : IPayloadSignal
        {
            Action<object> callbackAction = (callbackObject) =>
            {
                if (callbackObject is Action<TSignal> callback)
                    callback?.Invoke(signal);
                else
                    throw new Exception(_incorrectCallbackTypeMessage);
            };

            ApplyActionForCallback<TSignal>(callbackAction);
        }

        public void Invoke<TSignal>() where TSignal : ISimpleSignal
        {
            Action<object> callbackAction = (callbackObject) =>
            {
                if (callbackObject is Action callback)
                    callback?.Invoke();
                else
                    throw new Exception(_incorrectCallbackTypeMessage);
            };

            ApplyActionForCallback<TSignal>(callbackAction);
        }

        private void Subscribe<TSignal>(object callback)
        {
            string key = MakeKey<TSignal>();

            if (_signalCallbacks.ContainsKey(key))
                _signalCallbacks[key].Add(callback);
            else
                _signalCallbacks.Add(key, new List<object>() { callback });
        }

        private string MakeKey<TSignal>() =>
            typeof(TSignal).Name;

        private void Unsubscribe<TSignal>(object callback)
        {
            string key = MakeKey<TSignal>();

            if (_signalCallbacks.ContainsKey(key))
                _signalCallbacks[key].Remove(callback);
        }

        private void ApplyActionForCallback<TSignal>(Action<object> callbackAction)
        {
            string key = MakeKey<TSignal>();

            if (_signalCallbacks.ContainsKey(key))
            {
                foreach (object callbackObject in _signalCallbacks[key].ToList())
                    callbackAction.Invoke(callbackObject);
            }
        }
    }
}
