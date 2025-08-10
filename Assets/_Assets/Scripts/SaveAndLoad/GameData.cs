using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    int hp = 0;
    int atk = 0;
    int def = 0;
    List<ItemData> items = new List<ItemData>();
    List<DraggableItem> statusEquip = new List<DraggableItem>();
    List<DraggableItem> inventory = new List<DraggableItem>();
}
