using System;
using System.Linq;
using Zenject;

public class ActivePlayerHandler : IInitializable, IDisposable
{
    private SignalBus _signalBus;
    private IInput _input;
    private Player[] _players;
    private PlayerEscapeHandler _escapeHandler;

    private int _index = 0;

    public ActivePlayerHandler(SignalBus signalBus, Player[] players, PlayerEscapeHandler escapeHandler)
    {
        _signalBus = signalBus;
        _players = players;
        _escapeHandler = escapeHandler;
    }

    [Inject]
    protected void Construct(IInput input) => _input = input;

    public int GetPlayerIndex() => _index;

    public void Initialize()
    {
        SetActiveObject(0);
        _signalBus.Subscribe<GameEvents.PlayerEscaped>(SelectNextAvailablePlayer);
        _signalBus.Subscribe<GameEvents.LevelFinished>(OnMatchFinished);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.PlayerEscaped>(SelectNextAvailablePlayer);
        _signalBus.Unsubscribe<GameEvents.LevelFinished>(OnMatchFinished);
    }

    public void TrySelectPlayer(int index)
    {
        if (_escapeHandler.IsEscaped(_players[index]))
            return;
        SetActiveObject(index);
    }

    private void OnMatchFinished()
    {
        foreach (var p in _players)
            p.SetInput(null);
    }

    public void SelectNextAvailablePlayer()
    {
        if (_players.All(p => _escapeHandler.IsEscaped(p)))
            return;

        var index = _index;
        do
        {
            index = (index + 1) % _players.Length;
        }
        while (_escapeHandler.IsEscaped(_players[index]));
        SetActiveObject(index);
    }

    private void SetActiveObject(int index)
    {
        _index = index;
        for (int i = 0; i < _players.Length; i++)
            _players[i].SetInput(i == _index ? _input : null);

        _signalBus.Fire(new GameEvents.ActivePlayerChanged(_players[_index], _index));
    }
}
