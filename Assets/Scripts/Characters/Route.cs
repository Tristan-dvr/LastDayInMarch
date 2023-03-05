using UnityEngine;

public class Route : MonoBehaviour
{
    public Transform[] route = new Transform[0];

    [ContextMenu("Fill route")]
    private void FillRoute()
    {
        route = new Transform[transform.childCount];
        var index = 0;
        foreach (Transform child in transform)
            route[index++] = child;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < route.Length; i++)
        {
            var current = route[i];
            var nextIndex = i == route.Length - 1 ? 0 : i + 1;
            var next = route[nextIndex];
            if (current != null && next != null)
            {
                Gizmos.DrawSphere(current.position, 0.3f);
                Gizmos.DrawLine(current.position, next.position);
            }
        }
    }
}
