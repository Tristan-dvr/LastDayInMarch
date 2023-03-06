using UnityEngine.AddressableAssets;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [Inject]
    protected void Construct([Inject(Id = typeof(Bullet))] AssetReference bulletAsset)
    {
        Container.BindFactory<Bullet, Bullet.Factory>()
            .FromPoolableMemoryPool<Bullet, Bullet.Pool>(bind => bind
                .WithInitialSize(5)
                .FromComponentInNewPrefab(bulletAsset.Asset)
                .UnderTransformGroup("Bullets"));
    }

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
