using GameTemplate.Services.GameLevelLoader;
using Modules.SceneManagement;
using Zenject;

namespace GameTemplate.Installers.Project
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