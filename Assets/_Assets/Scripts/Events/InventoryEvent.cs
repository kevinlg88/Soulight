using UnityEngine;
using UnityEngine.Events;

public class InventoryEvent
{
    public UnityEvent<ItemData> OnItemSelected = new UnityEvent<ItemData>();
    public UnityEvent<ItemData> OnGearEquiped = new UnityEvent<ItemData>();
    public UnityEvent<ItemData> OnGearUnEquiped = new UnityEvent<ItemData>();
}
