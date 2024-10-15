using Modules.Analytics.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.Analytics.Configurations
{
    [CreateAssetMenu(fileName = "new AnalyticsConfiguration", menuName = "Modules/Analytics/AnalyticsConfiguration")]
    public class AnalyticsConfiguration : ScriptableObject
    {
        [Title("Game Load Settings")]
        [SerializeField] private DesignEventData _authorizationStageEvent = new ("GameBoot", "Authorization");
        [SerializeField] private DesignEventData _loadProgressStageEvent = new ("GameBoot", "LoadProgress");
        [SerializeField] private DesignEventData _mainMenuStageEvent = new ("GameBoot", "MainMenu");

        public DesignEventData AuthorizationStageEvent => _authorizationStageEvent;

        public DesignEventData LoadProgressStageEvent => _loadProgressStageEvent;

        public DesignEventData MainMenuStageEvent => _mainMenuStageEvent;
    }
}
