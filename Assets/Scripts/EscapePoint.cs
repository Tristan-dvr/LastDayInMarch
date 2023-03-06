using UnityEngine;
using Zenject;

public class EscapePoint : MonoBehaviour
{
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null 
            && other.attachedRigidbody.TryGetComponent<ICharacter>(out var character)
            && character.GetFaction() == Faction.Player)
        {
            _signalBus.Fire(new GameEvents.PlayerEscaped(character));
        }
    }
}
