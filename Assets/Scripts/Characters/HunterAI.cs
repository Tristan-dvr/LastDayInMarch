using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class HunterAI : MonoBehaviour, IInput
{
    public Character character;
    public float viewRange = 10;
    public float viewAngle = 160;

    private ICharacter target;
    private List<ICharacter> _characters;
    private PlayerEscapeHandler _escapeHandler;

    [Inject]
    protected void Construct(ICharacter[] characters, PlayerEscapeHandler escapeHandler)
    {
        _characters = characters.Where(c => Utils.IsEnemy(c, character)).ToList();
        _escapeHandler = escapeHandler;
    }

    private void Start()
    {
        character.SetInput(this);
    }

    private void FixedUpdate()
    {
        if (target != null && !_escapeHandler.IsEscaped(target))
            return;

        target = _characters.FirstOrDefault(IsReachable);
    }

    private bool IsReachable(ICharacter enemy)
    {
        if (_escapeHandler.IsEscaped(enemy))
            return false;

        var direction = enemy.GetPosition() - character.GetPosition();
        if (direction.sqrMagnitude > viewRange * viewRange)
            return false;

        var dot = Vector3.Dot(transform.forward, direction.normalized);
        var angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        return angle < viewAngle / 2;
    }

    public Vector2 GetMovement()
    {
        if (target == null)
            return Vector2.zero;

        var direction = (target.GetPosition() - character.GetPosition());
        return new Vector2(direction.x, direction.z).normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        var halfAngle = viewAngle / 2;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -halfAngle, 0) * transform.forward * viewRange);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewRange);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, halfAngle, 0) * transform.forward * viewRange);

        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(character.GetPosition(), target.GetPosition());
        }
    }
}
