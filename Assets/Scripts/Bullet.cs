using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour, IPoolable<IMemoryPool>
{
    public float speed = 1;

    private IMemoryPool _pool;
    private float _damage;
    public Faction _damageFaction;
    private Vector3 _velocity;
    private Rigidbody _parent;
    private Rigidbody _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction, float damage, Faction damageFaction, Rigidbody parent)
    {
        _velocity = direction.normalized * speed;
        _damage = damage;
        _damageFaction = damageFaction;
        _parent = parent;
    }

    private void FixedUpdate()
    {
        _body.velocity = _velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == _parent || (other.attachedRigidbody?.TryGetComponent<Bullet>(out _) ?? false))
            return;

        if (other.attachedRigidbody != null
            && other.attachedRigidbody.TryGetComponent<IDestructible>(out var destructible)
            && other.attachedRigidbody.TryGetComponent<ICharacter>(out var character) 
            && character.GetFaction() == _damageFaction)
            destructible.Damage(_damage);

        _pool.Despawn(this);
    }

    public void OnDespawned() { }

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }

    public class Factory : PlaceholderFactory<Bullet> { }
    public class Pool : MonoPoolableMemoryPool<IMemoryPool, Bullet> { }
}
