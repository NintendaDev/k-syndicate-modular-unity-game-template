using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace GameTemplate.Services.Analytics
{
    [Serializable]
    public class DesignEventData
    {
        private const string EventDelimeter = ":";

        [SerializeField, Required] private string _firstEventId;
        [SerializeField, Required] private string _secondEventId;
        [SerializeField] private string _thirdEventId;

        public DesignEventData(string firstEventId, string secondEventId, string thirdEventId) 
            : this(firstEventId, secondEventId)
        {
            _thirdEventId = thirdEventId;
        }

        public DesignEventData(string firstEventId, string secondEventId)
        {
            _firstEventId = firstEventId;
            _secondEventId = secondEventId;
        }

        public string FirstEventId => _firstEventId;

        public string SecondEventId => _secondEventId;

        public string ThirdEventId => _thirdEventId;

        public string GetEventName()
        {
            string eventName = _firstEventId;

            if (string.IsNullOrEmpty(_secondEventId))
                return eventName;

            eventName += EventDelimeter + _secondEventId;

            if (string.IsNullOrEmpty(_thirdEventId))
                return eventName;

            eventName += EventDelimeter + _thirdEventId;

            return eventName;
        } 
    }
}
