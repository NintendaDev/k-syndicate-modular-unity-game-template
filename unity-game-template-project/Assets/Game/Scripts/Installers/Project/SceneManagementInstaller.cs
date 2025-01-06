using Game.Services.GameLevelLoader;
using Modules.SceneManagement;
using Zenject;

namespace Game.Installers.Project
{
    public class SceneManagementInstaller : Installer<SceneManagementInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SingleSingleSceneLoader>().AsSingle();
            Container.BindInterfacesTo<LevelLoaderService>().AsSingle();
        }
    }
}