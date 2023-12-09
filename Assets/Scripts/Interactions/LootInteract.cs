using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LootInteract : MonoBehaviour, Interactable
{
    public List<Item> contains = new List<Item>();
    public int containedGold;

    public LootTable possibleLoot;

    public AudioClip clip;

    public GameObject lootedReplace;

    public void Start()
    {
        Randomize(possibleLoot);
    }

    public void Randomize(LootTable loot)
    {
        int numberOfItems = Random.Range(1, 4);
        for (int i = 0; i < numberOfItems; i++)
        {
            Item item = loot.loot[Random.Range(0, loot.loot.Length)];
            //Item item = new Item();
            contains.Add(item);
        }

        containedGold = Random.Range(1, 6);
    }

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
