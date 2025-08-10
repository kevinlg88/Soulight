using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    List<ItemData> items = new List<ItemData>();

    public List<ItemData> Items => items;
    public void AddItem(ItemData item)
    {
        items.Add(item);
    }
}
