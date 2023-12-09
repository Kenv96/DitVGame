using UnityEngine;

public class LockBottom : MonoBehaviour
{
    public Vector3 bottomPosition = new Vector3(0f, 0f, 0f); // Set the desired bottom position

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Lock the bottom position by setting it directly
        rb.position = bottomPosition;
    }
}
