using GameTemplate.UI.Wallets;
using System.Collections.Generic;

namespace GameTemplate
{
    public interface IWalletPanelView
    {
        public void Link(IEnumerable<WalletView> levelViews);
    }
}
