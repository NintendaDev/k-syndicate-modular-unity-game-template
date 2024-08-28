using Cysharp.Threading.Tasks;
using GameTemplate.Services.Localization;

namespace GameTemplate.Services.Popups
{
    public interface IPopupsService
    {
        public UniTask ShowInfoAsync(string messageHeader, string messageBody, string buttonText = "Ok");

        public UniTask ShowInfoAsync(LocalizationTerm headerTerm, LocalizationTerm messageTerm, LocalizationTerm buttonTerm);

        public UniTask ShowErrorAsync(string messageHeader, string messageBody, string buttonText = "Ok");

        public UniTask ShowErrorAsync(LocalizationTerm headerTerm, LocalizationTerm messageTerm, LocalizationTerm buttonTerm);
    }
}