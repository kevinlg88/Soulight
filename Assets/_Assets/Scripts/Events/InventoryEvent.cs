using UnityEngine;
using UnityEngine.Events;

public class InventoryEvent
{
    //Inventory events
    public UnityEvent<ItemData> OnItemAdded = new UnityEvent<ItemData>();
    public UnityEvent<ItemData> OnItemRemoved = new UnityEvent<ItemData>();
    public UnityEvent<ItemData> OnItemSelected = new UnityEvent<ItemData>();

    //Gear events
    public UnityEvent<ItemData> OnGearEquiped = new UnityEvent<ItemData>();
    public UnityEvent<ItemData> OnGearUnEquiped = new UnityEvent<ItemData>();

    //Items Events
    public UnityEvent<ItemData> OnItemUsed = new UnityEvent<ItemData>();
}
