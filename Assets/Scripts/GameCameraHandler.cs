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
        _signalBus.Subscribe<PlayerObjectsHandler.ActiveObjectChanged>(OnActiveObjectChanged);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<PlayerObjectsHandler.ActiveObjectChanged>(OnActiveObjectChanged);
    }

    private void OnActiveObjectChanged(PlayerObjectsHandler.ActiveObjectChanged data)
    {
        var player = _players[data.index];
        _camera.target = player.transform;
    }
}
