using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerPrefsStorage : IStorage
{
    private const string MetadataKey = "metadata";

    private Dictionary<int, GameStateMetadata> _metadata;
    private GameState _state;

    public async Task LoadFromSlotAsync(int slot)
    {
        var serializableMetadata = JsonUtility.FromJson<SerializableMetadata>(PlayerPrefs.GetString(MetadataKey));
        _metadata = serializableMetadata?.metadata ?? new Dictionary<int, GameStateMetadata>();
        await Task.Yield();
        _state = JsonUtility.FromJson<GameState>(PlayerPrefs.GetString(GetStatePrefsKey(slot))) ?? new GameState();
        await Task.Yield();
    }

    public async Task SaveToSlotAsync(int slot)
    {
        var serializableMetadata = new SerializableMetadata()
        {
            metadata = _metadata,
        };
        PlayerPrefs.SetString(MetadataKey, JsonUtility.ToJson(serializableMetadata));
        await Task.Yield();
        PlayerPrefs.SetString(GetStatePrefsKey(slot), JsonUtility.ToJson(_state));
        await Task.Yield();
    }

    private string GetStatePrefsKey(int slot) => $"save_slot_{slot}";

    public GameStateMetadata GetMetadata(int slot) => _metadata.TryGetValue(slot, out var metadata) ? metadata : null;

    public void SetMetadata(int slot, GameStateMetadata metadata) => _metadata[slot] = metadata;

    public GameState GetState() => _state;
}
