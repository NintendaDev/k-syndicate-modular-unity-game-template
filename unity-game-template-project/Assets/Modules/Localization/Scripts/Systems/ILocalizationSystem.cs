using System;

namespace Modules.Localization.Systems
{
    public interface ILocalizationSystem : ITranslation
    {
        public event Action LocalizationChanged;

        public void Initialize();
    }
}