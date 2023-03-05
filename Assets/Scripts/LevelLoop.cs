using System;
using System.Linq;
using Zenject;

public class LevelLoop : IInitializable, IDisposable
{
    private SignalBus _signalBus;
    private Player[] _players;
    private PlayerEscapeHandler _escapeHandler;

    public LevelLoop(SignalBus signalBus, Player[] players, PlayerEscapeHandler escapeHandler)
    {
        _signalBus = signalBus;
        _players = players;
        _escapeHandler = escapeHandler;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<GameEvents.PlayerEscaped>(OnPlayerEscaped);
        _signalBus.Subscribe<GameEvents.CharacterDamaged>(OnPlayerDamaged);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.PlayerEscaped>(OnPlayerEscaped);
        _signalBus.Unsubscribe<GameEvents.CharacterDamaged>(OnPlayerDamaged);
    }

    private void OnPlayerDamaged()
    {
        if (_players.Any(p => !p.IsAllive()))
            _signalBus.Fire(new GameEvents.LevelFinished(false));
    }

    private void OnPlayerEscaped()
    {
        if (_players.All(p => _escapeHandler.IsEscaped(p) && p.IsAllive()))
            _signalBus.Fire(new GameEvents.LevelFinished(true));
    }
}
