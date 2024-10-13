using Cysharp.Threading.Tasks;
using Modules.PopupsSystem.Configurations;
using Modules.PopupsSystem.UI.Simple;

namespace Modules.PopupsSystem.UI.Factories
{
    public interface IPopupFactory
    {
        public UniTask<SimplePopup> CreateInfoPopup(SimplePopupConfig config);

        public UniTask<SimplePopup> CreateErrorPopup(SimplePopupConfig config);
    }
}