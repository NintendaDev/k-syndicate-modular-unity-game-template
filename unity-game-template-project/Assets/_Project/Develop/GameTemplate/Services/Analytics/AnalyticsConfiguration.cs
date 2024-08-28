using Sirenix.OdinInspector;
using UnityEngine;

namespace GameTemplate.Services.Analytics
{
    [CreateAssetMenu(fileName = "new AnalyticsConfiguration", menuName = "GameTemplate/Analytics/AnalyticsConfiguration")]
    public class AnalyticsConfiguration : ScriptableObject
    {
        [Title("Game Load Settings")]
        [SerializeField] private DesignEventData _authorizationStageEvent = new DesignEventData("GameBoot", "Authorization");
        [SerializeField] private DesignEventData _loadProgressStageEvent = new DesignEventData("GameBoot", "LoadProgress");
        [SerializeField] private DesignEventData _preloadBannerStageEvent = new DesignEventData("GameBoot", "PreloadBanner");
        [SerializeField] private DesignEventData _mainMenuStageEvent = new DesignEventData("GameBoot", "MainMenu");

        public DesignEventData AuthorizationStageEvent => _authorizationStageEvent;

        public DesignEventData LoadProgressStageEvent => _loadProgressStageEvent;

        public DesignEventData PreloadBannerStageEvent => _preloadBannerStageEvent;

        public DesignEventData MainMenuStageEvent => _mainMenuStageEvent;
    }
}
