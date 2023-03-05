using UnityEngine;

[ExecuteAlways]
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 5;
    [Space]
    public Vector2 offset = new Vector2();

    private void Update()
    {
        if (target == null)
            return;

        var targetPosition = target.position + new Vector3(0, offset.y, offset.x);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
        transform.LookAt(target);
    }
}
