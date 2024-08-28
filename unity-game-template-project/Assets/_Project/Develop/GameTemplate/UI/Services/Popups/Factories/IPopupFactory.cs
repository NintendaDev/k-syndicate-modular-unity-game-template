using Cysharp.Threading.Tasks;
using GameTemplate.UI.Services.Popups.Simple;

namespace GameTemplate.UI.Services.Popups.Factories
{
    public interface IPopupFactory
    {
        public UniTask<SimplePopup> CreateInfoPopup(SimplePopupConfig config);

        public UniTask<SimplePopup> CreateErrorPopup(SimplePopupConfig config);
    }
}