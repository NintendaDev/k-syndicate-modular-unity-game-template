using Cysharp.Threading.Tasks;
using Modules.Localization.Types;

namespace Modules.PopupsSystem
{
    public interface IPopups
    {
        public UniTask ShowInfoAsync(string messageHeader, string messageBody, string buttonText = "Ok");

        public UniTask ShowInfoAsync(LocalizationTerm headerTerm, LocalizationTerm messageTerm, LocalizationTerm buttonTerm);

        public UniTask ShowErrorAsync(string messageHeader, string messageBody, string buttonText = "Ok");

        public UniTask ShowErrorAsync(LocalizationTerm headerTerm, LocalizationTerm messageTerm, LocalizationTerm buttonTerm);
    }
}