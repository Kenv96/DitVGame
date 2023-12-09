using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerInteract : MonoBehaviour, Interactable
{
    public List<Item> contains = new List<Item>();
    public int containedGold;

    public AudioClip clip;

    public GameObject lootedReplace;
    
    public void Interact()
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, transform.position, 0.4f);
        int itemsInChest = contains.Count;
        for (int i = 0; i < itemsInChest; i++)
        {
            if (Inventory.instance.Add(contains[0]))
            {
                contains.Remove(contains[0]);
            }
            else
            {
                break;
            }
        }
        Inventory.instance.AddGold(containedGold);
        containedGold = 0;
        if (contains.Count == 0)
        {
            if (lootedReplace != null)
                Instantiate(lootedReplace, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
