using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Kamikaze : Enemy
{
    private List<ICharacter> _characters;

    [Inject]
    protected void Construct(ICharacter[] characters)
    {
        _characters = characters.Where(c => Utils.IsEnemy(c, this)).ToList();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        foreach (var character in _characters)
        {
            if (Utils.InRange(GetPosition(), character.GetPosition(), attackRange) && character is IDestructible destructible)
            {
                destructible.Damage(damage);
                gameObject.SetActive(false);
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetPosition(), attackRange);
    }
}
