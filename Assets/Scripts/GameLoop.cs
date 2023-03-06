using System;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Zenject;

public class GameLoop : IInitializable, IDisposable
{
    private SaveGameHandler _storage;
    private LevelAssetsHandler _assetsHandler;
    private SignalBus _signalBus;
    private ZenjectSceneLoader _sceneLoader;
    private CoroutineHost _host;

    private int _loadedScene = -1;

    public GameLoop(
        SaveGameHandler storage,
        SignalBus signalBus,
        LevelAssetsHandler assetsHandler,
        ZenjectSceneLoader sceneLoader, 
        CoroutineHost host)
    {
        _storage = storage;
        _signalBus = signalBus;
        _assetsHandler = assetsHandler;
        _sceneLoader = sceneLoader;
        _host = host;
    }

    public async void Initialize()
    {
        await _assetsHandler.LoadAssetsAsync();
        await _storage.LoadDefault(1);
        LoadCurrentlevel();

        _signalBus.Subscribe<GameEvents.LevelFinished>(OnLevelFinished);
        _signalBus.Subscribe<StorageEvents.LoadSignal>(ReloadAndStartLevel);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.LevelFinished>(OnLevelFinished);
        _signalBus.Unsubscribe<StorageEvents.LoadSignal>(ReloadAndStartLevel);
    }

    private async void ReloadAndStartLevel(StorageEvents.LoadSignal data)
    {
        await _storage.LoadFromSlot(data.slot);
        LoadCurrentlevel();
    }

    private void OnLevelFinished(GameEvents.LevelFinished data)
    {
        if (data.success)
        {
            var nextIndex = Math.Max(1, (_loadedScene + 1) % SceneManager.sceneCountInBuildSettings);
            _storage.CurrentLevel = nextIndex;
        }
        LoadCurrentlevel();
    }

    private void LoadCurrentlevel()
    {
        _host.StartCoroutine(LoadScene(_storage.CurrentLevel));
    }

    private IEnumerator LoadScene(int index)
    {
        if (_loadedScene != -1)
            yield return SceneManager.UnloadSceneAsync(_loadedScene);

        yield return _sceneLoader.LoadSceneAsync(index, LoadSceneMode.Additive, containerMode: LoadSceneRelationship.Child);
        _loadedScene = index;
    }
}
