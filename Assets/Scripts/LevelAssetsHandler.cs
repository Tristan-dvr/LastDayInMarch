using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class LevelAssetsHandler
{
    private AssetReference _bulletAssetReference;

    public LevelAssetsHandler([Inject(Id = typeof(Bullet))] AssetReference bulletAssetReference)
    {
        _bulletAssetReference = bulletAssetReference;
    }

    public async Task LoadAssetsAsync()
    {
        await _bulletAssetReference.LoadAssetAsync<GameObject>().Task;
    }
}
