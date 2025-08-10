using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public ItemData itemData;
    public bool wasEquipped = false;
    [HideInInspector] public int positionInInventory;
    [HideInInspector] public Transform originalParent;

    void Awake()
    {
        positionInInventory = transform.GetSiblingIndex();
        if(transform.parent.GetComponent<Slot>().SlotType == EnumSlotType.Equipment) wasEquipped = true;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemData == null) return;
        if(transform.parent.GetComponent<Slot>().SlotType == EnumSlotType.Equipment) wasEquipped = true;
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
}
