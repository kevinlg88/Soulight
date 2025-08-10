using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image image;
    public ItemData itemData;
    public bool wasEquipped = false;
    [HideInInspector] public int positionInInventory;
    [HideInInspector] public Transform originalParent;

    private InventoryEvent _inventoryEvent;

    void Awake()
    {
        positionInInventory = transform.GetSiblingIndex();
        if(transform.parent.GetComponent<Slot>().SlotType == EnumSlotType.Equipment) wasEquipped = true;
    }

    [Inject]
    public void Construct(InventoryEvent inventoryEvent)
    {
        _inventoryEvent = inventoryEvent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemData == null) return;
        if (transform.parent.GetComponent<Slot>().SlotType == EnumSlotType.Equipment) wasEquipped = true;
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(itemData == null) return;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        image.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (transform.parent.GetComponent<Slot>().SlotType == EnumSlotType.Equipment) return;
            _inventoryEvent.OnItemUsed.Invoke(itemData);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            _inventoryEvent.OnItemSelected.Invoke(itemData);
        }
        // else if (eventData.button == PointerEventData.InputButton.Middle)
        // {
        //     Debug.Log("middle click detected!");
        // }
    }
}
