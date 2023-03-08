using UnityEngine;

public class RouteAI : MonoBehaviour, IInput
{
    private const float DistanceThreshold = 0.2f;

    public Character character;
    public Route route;

    private int _nextIndex;
    private Transform _next;

    private void Start()
    {
        _next = route.route[_nextIndex];
        character.SetInput(this);
    }

    private void Update()
    {
        if (Utils.InRange(_next.position, character.GetPosition(), DistanceThreshold))
        {
            _nextIndex = (_nextIndex + 1) % route.route.Length;
            _next = route.route[_nextIndex];
        }
    }

    public Vector2 GetMovement()
    {
        if (_next == null)
            return Vector2.zero;

        var direction = (_next.position - character.GetPosition()).normalized;
        return new Vector2(direction.x, direction.z);
    }

    private void OnDrawGizmosSelected()
    {
        if (route == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(character.GetPosition(), route.route[_nextIndex].position);
    }
}
