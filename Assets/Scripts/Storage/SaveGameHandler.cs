using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SaveGameHandler
{
    const int IgnoreSlot = -1;

    private IStorage _storage;
    private SignalBus _signalBus;

    public SaveGameHandler(IStorage storage, SignalBus signalBus)
    {
        _storage = storage;
        _signalBus = signalBus;
    }

    public int CurrentLevel
    {
        get => _storage.GetState().level;
        set => _storage.GetState().level = value;
    }

    public async Task LoadFromSlot(int slot)
    {
        await _storage.LoadFromSlotAsync(slot);
        _signalBus.Fire<StorageEvents.GameLoaded>();
    }

    public async Task LoadDefault(int firstLevel)
    {
        await _storage.LoadFromSlotAsync(IgnoreSlot);
        _storage.GetState().level = firstLevel;
    }

    public async void SaveToSlot(int slot)
    {
        if (slot == IgnoreSlot)
            return;

        CreateMetadata(slot);
        await _storage.SaveToSlotAsync(slot);
        _signalBus.Fire<StorageEvents.GameSaved>();
    }

    private void CreateMetadata(int slot)
    {
        var currentMetadata = _storage.GetMetadata(slot) ?? new GameStateMetadata();

        var state = _storage.GetState();
        currentMetadata.level = state.level;
        currentMetadata.playtime = Time.realtimeSinceStartup;

        _storage.SetMetadata(slot, currentMetadata);
    }

    public GameStateMetadata GetMetadata(int slot) => _storage.GetMetadata(slot);
}
