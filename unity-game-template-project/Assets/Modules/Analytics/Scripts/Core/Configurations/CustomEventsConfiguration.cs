using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Analytics.Types;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.Analytics.Configurations
{
    [CreateAssetMenu(fileName = "new CustomEventsConfiguration", 
        menuName = "Modules/Analytics/CustomEventsConfiguration")]
    public sealed class CustomEventsConfiguration : ScriptableObject
    {
        [SerializeField] private string _defaultParameterName = "value";
        
        [ValidateInput(nameof(IsUniqueEventData))]
        [SerializeField] private List<CustomEventData> _customEvents;
        
        public string DefaultParameterName => _defaultParameterName;

        public bool IsExistEventName(EventCode eventCode, AnalyticsSystemCode analyticsSystemCode, out string eventName)
        {
            eventName = _customEvents
                .Where(x => x.EventCode == eventCode)
                .SelectMany(x => x.EventNames)
                .Where(x => x.AnalyticsSystemCode == analyticsSystemCode)
                .Select(x => x.EventName)
                .FirstOrDefault();

            return string.IsNullOrEmpty(eventName) == false;
        }

        private bool IsUniqueEventData(List<CustomEventData> customEvents, ref string errorMessage)
        {
            if (customEvents.GroupBy(x => x.EventCode).Count() != customEvents.Count)
            {
                errorMessage = "Duplicate events codes found";

                return false;
            }

            return true;
        }

        [Serializable]
        private struct CustomEventData
        {
            [ValidateInput(nameof(IsNotNoneEventCode))]
            [SerializeField] public EventCode _eventCode;

            [ValidateInput(nameof(IsUniqueEventName))] 
            [RequiredListLength(1, null)]
            [SerializeField] public List<CustomEventName> _eventNames;

            public EventCode EventCode => _eventCode;

            public List<CustomEventName> EventNames => _eventNames;

            private bool IsUniqueEventName(List<CustomEventName> eventNames, ref string errorMessage)
            {
                if (eventNames.GroupBy(x => x.AnalyticsSystemCode).Count() != eventNames.Count)
                {
                    errorMessage = "Duplicate analytics system codes found";

                    return false;
                }

                return true;
            }

            private bool IsNotNoneEventCode(EventCode eventCode, ref string errorMessage)
            {
                if (eventCode == EventCode.None)
                {
                    errorMessage = "The event code cannot be None";

                    return false;
                }

                return true;
            }
        }
        
        [Serializable]
        private struct CustomEventName
        {
            [FormerlySerializedAs("_systemCode")]
            [ValidateInput(nameof(IsNotNoneAnalyticsSystemCode))] 
            [SerializeField] private AnalyticsSystemCode _analyticsSystemCode;
            
            [Required] 
            [SerializeField] private string _eventName;
            
            public AnalyticsSystemCode AnalyticsSystemCode => _analyticsSystemCode;
            
            public string EventName => _eventName;
            
            private bool IsNotNoneAnalyticsSystemCode(AnalyticsSystemCode systemCode, ref string errorMessage)
            {
                if (systemCode == AnalyticsSystemCode.None)
                {
                    errorMessage = "The analytics system code cannot be None";

                    return false;
                }

                return true;
            }
        }
    }
}