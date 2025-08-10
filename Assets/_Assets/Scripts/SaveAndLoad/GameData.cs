using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int hp = 0;
    public int atk = 0;
    public int def = 0;
    public List<ItemUI> statusEquip = new List<ItemUI>();
    public List<ItemUI> inventory = new List<ItemUI>();

    public GameData(InventoryView inventoryView)
    {
        hp = inventoryView.GetHp();
        atk = inventoryView.GetAtk();
        def = inventoryView.GetDef();
        statusEquip = inventoryView.GetStatusEquip();
        inventory = inventoryView.GetInventory();
    }
}

