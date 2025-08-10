using Zenject;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EnumSlotType
{
    Inventory,
    Equipment
}

public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] EnumSlotType slotType;

    public EnumSlotType SlotType => slotType;
    private InventoryEvent _inventoryEvent;
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectItem);
    }

    [Inject]
    public void Construct(InventoryEvent inventoryEvent)
    {
        _inventoryEvent = inventoryEvent;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if(slotType == EnumSlotType.Equipment &&
            eventData.pointerDrag.GetComponent<DraggableItem>().itemData is ConsumableData)
            return;

        DraggableItem currentItem = transform.GetChild(0).GetComponent<DraggableItem>();
        if (currentItem.itemData == null)
        {
            GameObject droppedItem = eventData.pointerDrag;
            DraggableItem draggable = droppedItem.GetComponent<DraggableItem>();
            if (slotType == EnumSlotType.Equipment)
            {
                _inventoryEvent.OnGearEquiped.Invoke(draggable.itemData);
            }
            if (draggable.wasEquipped)
            {
                _inventoryEvent.OnGearUnEquiped.Invoke(draggable.itemData);
            }
            SetDraggableItem(currentItem, draggable);
            ClearDraggableItem(draggable);

        }
        else
        {
            GameObject droppedItem = eventData.pointerDrag;
            DraggableItem draggable = droppedItem.GetComponent<DraggableItem>();
            Debug.Log($"OnDrop: {currentItem.itemData.name} and {draggable.itemData.name}");
            if (slotType == EnumSlotType.Equipment)
            {
                _inventoryEvent.OnGearUnEquiped.Invoke(currentItem.itemData);
                _inventoryEvent.OnGearEquiped.Invoke(draggable.itemData);
            }
            if (draggable.wasEquipped)
            {
                _inventoryEvent.OnGearUnEquiped.Invoke(draggable.itemData);
            }
            SwapItems(currentItem, draggable);
        }

    }

    private void SwapItems(DraggableItem currentItem, DraggableItem draggable)
    {
        var tempItemData = currentItem.itemData;
        var tempImage = currentItem.image.sprite;
        var tempColor = currentItem.image.color;
        SetDraggableItem(currentItem, draggable);
        draggable.itemData = tempItemData;
        draggable.image.sprite = tempImage;
        draggable.image.color = tempColor;
    }

    void SetDraggableItem(DraggableItem currentItem, DraggableItem draggable)
    {
        currentItem.itemData = draggable.itemData;
        currentItem.image.sprite = draggable.image.sprite;
        currentItem.image.color = draggable.image.color;
    }
    void ClearDraggableItem(DraggableItem draggable)
    {
        draggable.itemData = null;
        draggable.image.sprite = null;
        draggable.image.color = Color.clear;
    }

    void SelectItem()
    {
        if(slotType != EnumSlotType.Inventory)return;
        DraggableItem draggableItem = GetComponentInChildren<DraggableItem>();
        Debug.Log($"SelectItem: {draggableItem.itemData?.name ?? "No item"}");
        _inventoryEvent.OnItemSelected.Invoke(draggableItem.itemData);
    }
}
