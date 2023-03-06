using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [Inject]
    protected void Construct([Inject(Id = typeof(Bullet))] AssetReference bulletAsset)
    {
        Assert.IsNotNull(bulletAsset.Asset, "Bullet asset not loaded yet!");

        Container.BindFactory<Bullet, Bullet.Factory>()
            .FromPoolableMemoryPool<Bullet, Bullet.Pool>(bind => bind
                .WithInitialSize(5)
                .FromComponentInNewPrefab(bulletAsset.Asset)
                .UnderTransformGroup("Bullets"));
    }

    public override void InstallBindings()
    {

        Container.BindInterfacesAndSelfTo<GamePauseHandler>().AsCached();
        Container.BindExecutionOrder<GamePauseHandler>(int.MinValue);

        Container.BindInterfacesAndSelfTo<GameCameraHandler>().AsCached().NonLazy();

        Container.BindInterfacesAndSelfTo<PlayerEscapeHandler>().AsCached();
        Container.BindInterfacesAndSelfTo<ActivePlayerHandler>().AsCached();
        Container.BindInterfacesTo<KeyboardInput>().AsCached().IfNotBound();

        Container.BindInterfacesTo<LevelLoop>().AsCached().NonLazy();

        Container.Bind<UnityEventsHandler>().FromNewComponentOnNewGameObject().AsCached().NonLazy();
    }
}
