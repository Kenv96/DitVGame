using System.Collections.Generic;
using UnityEngine;



public class RandomLoot
{
    public static List<Item> Randomize(LootTable loot)
    {
        int numberOfItems = Random.Range(1, 4);
        List<Item> drops = new List<Item>();
        for(int i = 0; i < numberOfItems;i++)
        {
            Item item = loot.loot[Random.Range(0,loot.loot.Length)];
            //Item item = new Item();
            drops.Add(item);
        }
        return drops;
    }
}
