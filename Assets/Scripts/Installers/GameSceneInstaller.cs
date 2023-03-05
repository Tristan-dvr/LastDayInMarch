using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<CoroutineHost>().FromNewComponentOnNewGameObject().AsCached();
        Container.BindInterfacesTo<GameLoop>().AsCached();
    }
}
