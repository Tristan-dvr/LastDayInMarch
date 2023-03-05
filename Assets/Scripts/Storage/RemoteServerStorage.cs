using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RemoteServerStorage : IStorage
{
    private GameState _state;
    private Dictionary<int, GameStateMetadata> _metadata = new Dictionary<int, GameStateMetadata>();

    public GameStateMetadata GetMetadata(int slot) => _metadata.TryGetValue(slot, out var metadata) ? metadata : null;

    public void SetMetadata(int slot, GameStateMetadata metadata) => _metadata[slot] = metadata;

    public GameState GetState() => _state;


    public async Task LoadFromSlotAsync(int slot)
    {
        var stateJson = await LoadString($"state_url?slot={slot}");
        _state = JsonUtility.FromJson<GameState>(stateJson);
        var metadataJson = await LoadString("metadata_url");
        var metadataDto = JsonUtility.FromJson<SerializableMetadata>(metadataJson);
        _metadata = metadataDto.metadata;
    }

    public async Task SaveToSlotAsync(int slot)
    {
        var dto = new GameStateDto()
        {
            slot = slot,
            state = _state,
            metadata = GetMetadata(slot),
        };
        var json = JsonUtility.ToJson(dto);
        await PostString("post_state_url", json);
    }

    private Task<string> LoadString(string url) => Task.FromResult<string>(default);
    private Task PostString(string url, string json) => Task.CompletedTask;

    [Serializable]
    private class GameStateDto
    {
        public int slot;
        public GameState state;
        public GameStateMetadata metadata;
    }
}
