using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.Install<GameEvents>();
        Container.DeclareSignal<PlayerObjectsHandler.ActiveObjectChanged>();

        Container.Install<StorageEvents>();
        Container.Bind<IStorage>().To<PlayerPrefsStorage>().AsCached();
        Container.BindInterfacesAndSelfTo<SaveGameHandler>().AsCached();
    }
}
