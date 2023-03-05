using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableMetadata : ISerializationCallbackReceiver
{
    [NonSerialized]
    public Dictionary<int, GameStateMetadata> metadata = new Dictionary<int, GameStateMetadata>();

    [SerializeField]
    private int[] _keys = new int[0];
    [SerializeField]
    private string[] _values = new string[0];

    public void OnAfterDeserialize()
    {
        metadata.Clear();
        for (int i = 0; i < _keys.Length; i++)
            metadata[_keys[i]] = JsonUtility.FromJson<GameStateMetadata>(_values[i]);
    }

    public void OnBeforeSerialize()
    {
        _keys = new int[metadata.Count];
        _values = new string[metadata.Count];
        var i = 0;
        foreach (var element in metadata)
        {
            _keys[i] = element.Key;
            _values[i] = JsonUtility.ToJson(element.Value);
            i++;
        }
    }
}
