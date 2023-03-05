using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class PlayerObjectsHandler : IInitializable, IDisposable
{
    private SignalBus _signalBus;
    private IInput _input;
    private List<ICharacter> _playerObjects;
    private PlayerEscapeHandler _escapeHandler;

    private int _index = 0;

    public PlayerObjectsHandler(SignalBus signalBus, IInput input, ICharacter[] characters, PlayerEscapeHandler escapeHandler)
    {
        _signalBus = signalBus;
        _input = input;
        _playerObjects = characters.Where(c => c.GetFaction() == Faction.Player).ToList();
        _escapeHandler = escapeHandler;
    }

    public int GetActiveIndex() => _index;

    public void Initialize()
    {
        _index = -1;
        SelectAvailablePlayer();
        _signalBus.Subscribe<GameEvents.PlayerEscaped>(SelectAvailablePlayer);
        _signalBus.Subscribe<GameEvents.LevelFinished>(OnMatchFinished);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.PlayerEscaped>(SelectAvailablePlayer);
        _signalBus.Unsubscribe<GameEvents.LevelFinished>(OnMatchFinished);
    }

    public void TrySelectPlayer(int index)
    {
        if (_escapeHandler.IsEscaped(_playerObjects[index]))
            return;
        SetActiveObject(index);
    }

    private void OnMatchFinished()
    {
        _playerObjects.ForEach(p => p.SetInput(null));
    }

    public void SelectAvailablePlayer()
    {
        if (_playerObjects.All(p => _escapeHandler.IsEscaped(p)))
            return;

        var index = _index;
        do
        {
            index = (index + 1) % _playerObjects.Count;
        }
        while (_escapeHandler.IsEscaped(_playerObjects[index]));
        SetActiveObject(index);
    }

    private void SetActiveObject(int index)
    {
        _index = index;
        for (int i = 0; i < _playerObjects.Count; i++)
            _playerObjects[i].SetInput(i == _index ? _input : null);

        _signalBus.Fire(new ActiveObjectChanged(_playerObjects[_index], _index));
    }

    public struct ActiveObjectChanged
    {
        public int index;
        public IControllable newActive;

        public ActiveObjectChanged(IControllable newActive, int index)
        {
            this.newActive = newActive;
            this.index = index;
        }
    }
}
