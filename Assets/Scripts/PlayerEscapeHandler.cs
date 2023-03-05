using System;
using System.Collections.Generic;
using Zenject;

public class PlayerEscapeHandler : IInitializable, IDisposable
{
    private SignalBus _signalBus;
    private HashSet<IControllable> _escapedPlayers = new HashSet<IControllable>();

    public PlayerEscapeHandler(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public bool IsEscaped(IControllable controllable) => _escapedPlayers.Contains(controllable);

    public void Initialize()
    {
        _signalBus.Subscribe<GameEvents.PlayerEscaped>(OnPlayerEscaped);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.PlayerEscaped>(OnPlayerEscaped);
    }

    private void OnPlayerEscaped(GameEvents.PlayerEscaped data)
    {
        _escapedPlayers.Add(data.player);
    }
}
