using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteract : MonoBehaviour, Interactable
{
    public Item item;

    public AudioClip clip;
    public void Interact()
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
        Debug.Log("Picking up item");
        bool wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp) Destroy(gameObject);
    }
}
