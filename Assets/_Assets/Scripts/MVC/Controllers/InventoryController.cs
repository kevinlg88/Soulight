using UnityEngine;

public class InventoryController
{
    private InventoryModel _inventoryModel;
    private InventoryEvent _inventoryEvent;

    public InventoryController()
    {
        _inventoryModel = new InventoryModel();
        Debug.Log("InventoryController initialized with an empty inventory model.");
    }

    public void SetInventoryEvent(InventoryEvent inventoryEvent)
    {
        _inventoryEvent = inventoryEvent;
        _inventoryEvent.OnItemUsed.AddListener(UseItem);
    }

    public void AddItem(ItemData item)
    {
        _inventoryModel.AddItem(item);
        _inventoryEvent.OnItemAdded.Invoke(item);
    }
    public void RemoveItem(ItemData item)
    {
        _inventoryModel.RemoveItem(item);
        _inventoryEvent.OnItemRemoved.Invoke(item);
    }

    private void UseItem(ItemData item)
    {
        if (_inventoryModel.Items.Contains(item))
        { 
            RemoveItem(item);
        }

    }
}
