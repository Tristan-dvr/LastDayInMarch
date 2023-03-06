using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character, IDestructible
{
    [Space]
    public string characterName;
    public float health = 50;

    private SignalBus _signalBus;

    private float _health;

    protected override void Awake()
    {
        base.Awake();
        _health = health;
    }

    [Inject]
    protected void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnDrawGizmos()
    {
        if (_body == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _body.velocity);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * speed);
    }

    public float GetHealth() => _health;

    public void Damage(float damage)
    {
        if (!IsAllive())
            return;

        _health -= damage;
        if (_health < 0)
            _health = 0;

        _signalBus.Fire(new GameEvents.PlayerDamaged(this, damage));
    }

    public bool IsAllive() => _health > 0;

    public override Faction GetFaction() => Faction.Player;
}
