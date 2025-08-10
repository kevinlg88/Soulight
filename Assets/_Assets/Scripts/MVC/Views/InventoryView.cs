using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryView : MonoBehaviour
{
    [Header("Description Panel")]
    [SerializeField] GameObject descriptionPanel;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemDescriptionText;
    [SerializeField] Image itemIconImage;


    [Inject]
    public void Construct(InventoryEvent inventoryEvent)
    {
        inventoryEvent.OnItemSelected.AddListener(ShowItemDescription);
    }

    void ShowItemDescription(ItemData itemData)
    {
        if (itemData == null)
        {
            descriptionPanel.SetActive(false);
            itemNameText.text = string.Empty;
            itemDescriptionText.text = string.Empty;
            itemIconImage.sprite = null;
            Debug.Log("No item selected, hiding description panel.");
            return;
        }
        descriptionPanel.SetActive(true);
        itemNameText.text = itemData.itemName;
        itemDescriptionText.text = itemData.description;
        itemIconImage.sprite = itemData.itemIcon;
        Debug.Log($"Showing description for item: {itemData.itemName}");
    }
}
