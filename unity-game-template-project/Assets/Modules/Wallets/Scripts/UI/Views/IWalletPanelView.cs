using System.Collections.Generic;

namespace Modules.Wallets.UI.Views
{
    public interface IWalletPanelView
    {
        public void Link(IEnumerable<WalletView> levelViews);
    }
}
