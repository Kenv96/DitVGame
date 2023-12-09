using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image icon;

    Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled= true;
    }

    public void Clear()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }
}
