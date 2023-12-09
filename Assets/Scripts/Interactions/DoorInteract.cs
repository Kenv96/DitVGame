using UnityEngine;

public class DoorInteract : MonoBehaviour, Interactable
{
    public AudioClip clip;

    public void Interact()
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
        Debug.Log("Opening door");
        Destroy(gameObject);
    }
}
