using System;
using UnityEngine;
using Zenject;

public class GamePauseHandler : IInitializable, IDisposable
{
    private SignalBus _signalBus;

    public GamePauseHandler(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public bool Paused { get; private set; }

    public void Initialize()
    {
        _signalBus.Subscribe<GameEvents.Pause>(OnGamePause);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.Pause>(OnGamePause);
    }

    private void OnGamePause(GameEvents.Pause data)
    {
        Debug.Log($"Pause {data.pause}");
        Time.timeScale = data.pause ? 0 : 1;
        Paused = data.pause;
    }
}
