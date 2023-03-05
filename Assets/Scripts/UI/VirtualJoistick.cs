using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class VirtualJoistick : MonoBehaviour, IInput, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public RectTransform bounds;
    public RectTransform knob;

    private bool _pressed;
    private Vector2 _touchPosition;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Start()
    {
        _touchPosition = bounds.rect.center;
        _signalBus.Subscribe<GameEvents.Pause>(OnPause);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<GameEvents.Pause>(OnPause);
    }

    private void OnPause()
    {
        _pressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
        _touchPosition = eventData.position;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        _touchPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
        _touchPosition = eventData.position;
    }

    private void Update()
    {
        if (!_pressed)
        {
            knob.position = bounds.position;
            return;
        }

        knob.position = _touchPosition;
    }

    public Vector2 GetMovement()
    {
        if (!_pressed)
            return Vector2.zero;

        var position = knob.anchoredPosition;
        var movement = position / (bounds.rect.size / 2);
        return movement.sqrMagnitude > 1 ? movement.normalized : movement;

    }
}
