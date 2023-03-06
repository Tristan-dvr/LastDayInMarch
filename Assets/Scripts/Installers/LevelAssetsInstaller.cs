using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

[CreateAssetMenu(fileName = "LevelAssets", menuName = "Game/LevelAssets")]
public class LevelAssetsInstaller : ScriptableObjectInstaller
{
    public AssetReference bullet;

    public override void InstallBindings()
    {
        Container.BindInstance(bullet).WithId(typeof(Bullet)).AsCached();
        Container.Bind<LevelAssetsHandler>().AsCached();
    }
}
