using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SaveGameUI : MonoBehaviour, IInitializable, IDisposable
{
    public SaveGameSlot template;

    private SaveGameHandler _storage;
    private SignalBus _signalBus;

    private List<SaveGameSlot> _slots = new List<SaveGameSlot>();

    private void Awake()
    {
        template.gameObject.SetActive(false);
    }

    [Inject]
    public void Construct(SaveGameHandler storage, SignalBus signalBus)
    {
        _storage = storage;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        CreateSlots();
        RefreshSlots();

        _signalBus.Subscribe<StorageEvents.GameSaved>(RefreshSlots);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<StorageEvents.GameSaved>(RefreshSlots);
    }

    private void CreateSlots()
    {
        for (int i = 0; i < 3; i++)
        {
            var index = i;
            var slot = Instantiate(template, template.transform.parent);
            slot.save.onClick.AddListener(() => _storage.SaveToSlot(index));
            slot.load.onClick.AddListener(() =>
            {
                _signalBus.Fire(new GameEvents.Pause(false));
                _signalBus.Fire(new StorageEvents.LoadSignal(index));
            });
            slot.gameObject.SetActive(true);
            _slots.Add(slot);
        }
    }

    private void RefreshSlots()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            var slot = _slots[i];
            var metadata = _storage.GetMetadata(i);
            if (metadata != null)
            {
                slot.active.enabled = true;
                slot.load.interactable = true;
                slot.text.text = $"Level: {metadata.level}\nPlaytime: {(int)metadata.playtime} sec";
            }
            else
            {
                slot.active.enabled = false;
                slot.load.interactable = false;
                slot.text.text = "empty";
            }
        }
    }
}
