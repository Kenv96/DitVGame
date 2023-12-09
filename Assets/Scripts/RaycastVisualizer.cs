using UnityEngine;

public class RaycastVisualizer : MonoBehaviour
{
    public Transform origin; // The starting point of the ray
    public Vector3 direction = Vector3.down; // The direction of the ray
    Vector3 offset = new Vector3(0, 1f, 0);
    public float rayLength = 10.0f; // The length of the ray

    void OnDrawGizmosSelected()
    {
        if (origin == null)
            return;

        // Set the color for the ray visualization
        Gizmos.color = Color.yellow;

        // Draw the ray using Gizmos
        Gizmos.DrawRay(origin.position + offset, direction * rayLength);
    }
}

