using Zenject;

public class LevelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<KeyboardInput>().AsCached().IfNotBound();

        Container.BindInterfacesAndSelfTo<GamePauseHandler>().AsCached();
        Container.BindExecutionOrder<GamePauseHandler>(int.MinValue);

        Container.BindInterfacesAndSelfTo<GameCameraHandler>().AsCached().NonLazy();

        Container.BindInterfacesAndSelfTo<PlayerEscapeHandler>().AsCached();

        Container.BindInterfacesAndSelfTo<PlayerObjectsHandler>().AsCached();
        Container.BindInterfacesTo<LevelLoop>().AsCached().NonLazy();

        Container.Bind<UnityEventsHandler>().FromNewComponentOnNewGameObject().AsCached().NonLazy();
    }
}
