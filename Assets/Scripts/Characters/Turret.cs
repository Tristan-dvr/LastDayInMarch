using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Turret : Enemy
{
    [Space]
    public float fireRate = 1;
    public Transform firePoint;

    private Bullet.Factory _bulletsFactory;
    private float _lastAttackTime;

    private List<ICharacter> _characters;

    [Inject]
    public void Construct(Bullet.Factory bulletsFactory, ICharacter[] characters)
    {
        _bulletsFactory = bulletsFactory;
        _characters = characters.Where(c => Utils.IsEnemy(c, this)).ToList();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Time.timeSinceLevelLoad - _lastAttackTime < 1 / fireRate)
            return;

        var sqrRange = attackRange * attackRange;
        foreach (var character in _characters)
        {
            var direction = character.GetPosition() - GetPosition();
            if (direction.sqrMagnitude < sqrRange)
            {
                FireTo(direction);
                _lastAttackTime = Time.timeSinceLevelLoad;
                break;
            }
        }
    }

    private void FireTo(Vector3 direction)
    {
        var bullet = _bulletsFactory.Create();
        bullet.transform.position = firePoint.position;
        bullet.Launch(direction, damage, Faction.Player, _body);
        Debug.DrawLine(firePoint.position, firePoint.position + direction, Color.red, 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetPosition(), attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(GetPosition(), GetPosition() + transform.forward);
    }
}
