using System;

namespace Modules.Localization.Core.Systems
{
    public interface ILocalizationSystem : ITranslation
    {
        public event Action LocalizationChanged;

        public void Initialize();
    }
}