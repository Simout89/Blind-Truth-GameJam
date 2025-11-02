using Zenject;
using Скриптерсы.View;

namespace Скриптерсы.Zenject
{
    public class GamePlaySceneInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NoteView>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateManager>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraController>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInteraction>().FromComponentsInHierarchy().AsSingle();

        }
    }
}