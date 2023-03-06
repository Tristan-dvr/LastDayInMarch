using UnityEngine;

public static class Utils
{
    public static bool IsEnemy(ICharacter character1, ICharacter character2)
    {
        return character1.GetFaction() != character2.GetFaction();
    }

    public static bool InRange(Vector3 center, Vector3 point, float radius)
    {
        var direction = point - center;
        return direction.sqrMagnitude < radius * radius;
    }
}
