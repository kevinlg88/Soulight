using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int hp = 0;
    public int atk = 0;
    public int def = 0;
    //public List<ItemData> items = new List<ItemData>();
    public List<DraggableItem> statusEquip = new List<DraggableItem>();
    public List<DraggableItem> inventory = new List<DraggableItem>();

    public GameData(InventoryView inventoryView)
    {
    }
}
