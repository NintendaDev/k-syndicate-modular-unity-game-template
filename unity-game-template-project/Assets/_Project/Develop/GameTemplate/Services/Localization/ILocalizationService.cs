using System;

namespace GameTemplate.Services.Localization
{
    public interface ILocalizationService : ITranslation
    {
        public event Action LocalizationChanged;

        public void Initialize();
    }
}