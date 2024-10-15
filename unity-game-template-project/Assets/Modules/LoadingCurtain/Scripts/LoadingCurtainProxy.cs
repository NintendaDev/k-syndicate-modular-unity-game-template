using Cysharp.Threading.Tasks;

namespace Modules.LoadingCurtain
{
    public class LoadingCurtainProxy : ILoadingCurtain
    {
        private readonly LoadingCurtainFabric _factory;
        private ILoadingCurtain _curtain;

        public LoadingCurtainProxy(LoadingCurtainFabric factory)
        {
            _factory = factory;
        }
            
        public async UniTask InitializeAsync() => 
            _curtain = await _factory.CreateAsync();

        public void Show() => _curtain.Show();

        public void Hide() => _curtain.Hide();
    }
}