using System;
using Zenject;

public class GameCameraHandler : IInitializable, IDisposable
{
    private CameraFollow _camera;
    private SignalBus _signalBus;
    private Player[] _players;

    public GameCameraHandler(CameraFollow camera, SignalBus signalBus, Player[] players)
    {
        _camera = camera;
        _signalBus = signalBus;
        _players = players;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<GameEvents.ActivePlayerChanged>(OnActiveObjectChanged);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.ActivePlayerChanged>(OnActiveObjectChanged);
    }

    private void OnActiveObjectChanged(GameEvents.ActivePlayerChanged data)
    {
        var player = _players[data.index];
        _camera.target = player.transform;
    }
}
