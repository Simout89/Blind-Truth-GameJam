using Zenject;
using Скриптерсы.Services;

namespace Скриптерсы.Zenject
{
    public class ProjectInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputService>()
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<SaveRepository>()
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<SettingService>()
                .AsSingle()
                .NonLazy();
        }
    }
}