using UnityEngine;
using Zenject;

public class KeyboardInput : IInput, ITickable
{
    private ActivePlayerHandler _handler;

    [Inject]
    protected void Construct(ActivePlayerHandler handler)
    {
        _handler = handler;
    }

    public Vector2 GetMovement()
    {
        var movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        return movement.sqrMagnitude > 1 ? movement.normalized : movement;
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            _handler.SelectNextAvailablePlayer();
    }
}
