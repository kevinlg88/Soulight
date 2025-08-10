using UnityEngine;
using Zenject;

public class PlayerInventory : MonoBehaviour
{
    private InventoryController _inventoryController;

    [SerializeField] int maxInventoryItens = 8;
    [SerializeField] int countItems = 0;

    public bool isInventoryFull = false;

    [Inject]
    public void Construct(InventoryController inventoryController)
    {
        _inventoryController = inventoryController;
    }

    public void AddItemToInventory(ItemData itemData)
    {
        _inventoryController.AddItem(itemData);
        countItems++;
        if(countItems >= maxInventoryItens) isInventoryFull = true;
        else isInventoryFull = false;
    }

    public void RemoveItem(ItemData itemData)
    {
        _inventoryController.RemoveItem(itemData);
        countItems--;
        isInventoryFull = false;
    }
}