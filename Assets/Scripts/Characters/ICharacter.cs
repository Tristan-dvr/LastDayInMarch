using UnityEngine;

public interface ICharacter : IControllable
{
    Faction GetFaction();
    Vector3 GetPosition();
    Vector3 GetViewDirection();
}
