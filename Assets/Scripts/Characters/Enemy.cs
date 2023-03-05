using UnityEngine;

public class Enemy : Character
{
    [Space]
    public float damage;
    public float attackRange = 10;

    public override Faction GetFaction() => Faction.Enemy;
}
