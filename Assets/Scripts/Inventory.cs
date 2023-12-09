using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    int space = 10;
    
    public int coins;
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }

        instance = this;
    }

    public bool Add(Item item)
    {
        if(items.Count >= space)
        {
            Debug.Log("Not enough bag space");
            return false;
        }
        items.Add(item);
        QuestManager.GetInstance().CheckQuestItem(item);
        if(onItemChangedCallback!= null)
            onItemChangedCallback.Invoke();

        return true;
    }

    public void AddGold(int gold)
    {
        coins += gold;
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void RemoveMultiple(Item item, int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            items.Remove(item);
            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }

    public bool CheckInv(Item item, int amount)
    {
        int count = 0;
        foreach(Item item2 in items) 
        {
            if(item2 == item) count++;
        }
        if(count >= amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
