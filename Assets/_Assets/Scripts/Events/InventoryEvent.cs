using UnityEngine;
using UnityEngine.Events;

public class InventoryEvent
{
    public UnityEvent<ItemData> OnItemSelected = new UnityEvent<ItemData>();
}
