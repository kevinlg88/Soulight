using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public ItemData itemData;
    [HideInInspector] public int positionInInventory;
    [HideInInspector] public Transform originalParent;

    void Awake()
    {
        positionInInventory = transform.GetSiblingIndex();  
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemData == null) return;
        Debug.Log("OnBeginDrag");
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(itemData == null) return;
        Debug.Log("OnDrag");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        transform.SetParent(originalParent);
        image.raycastTarget = true;
    }
}
