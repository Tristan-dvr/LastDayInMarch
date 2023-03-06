using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Character : MonoBehaviour, ICharacter
{
    public bool movable = true;
    public float speed = 1;

    private IInput _input;
    protected Rigidbody _body;

    protected virtual void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _body.isKinematic = true;
    }

    protected virtual void FixedUpdate()
    {
        if (!IsUnderControl())
            return;

        var movement = GetInput().GetMovement();
        var movementVector = new Vector3(movement.x, 0, movement.y);
        _body.velocity = new Vector3(movement.x, 0, movement.y) * speed;

        if (movementVector.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementVector), Time.fixedDeltaTime * speed * 5);
    }

    public abstract Faction GetFaction();

    public Vector3 GetPosition() => transform.position;

    public virtual bool IsUnderControl() => _input != null && movable;

    public virtual void SetInput(IInput input)
    {
        _input = input;
        _body.isKinematic = !IsUnderControl();
    }

    protected IInput GetInput() => _input;

    public Vector3 GetViewDirection() => transform.forward;
}
