using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    [Header("Status Panel")]
    [SerializeField] GameObject statusPanel;
    [SerializeField] TextMeshProUGUI healthValueText;
    [SerializeField] TextMeshProUGUI atkValueText;
    [SerializeField] TextMeshProUGUI defValueText;
    [SerializeField] List<Slot> slots = new List<Slot>();

    [Header("Inventory Panel")]
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] List<Slot> inventorySlots = new List<Slot>();

    private InventoryController _inventoryController;


    [Inject]
    public void Construct(InventoryEvent inventoryEvent)
    {
        //Inventory events
        inventoryEvent.OnItemAdded.AddListener(AddItemToInventory);
        inventoryEvent.OnItemRemoved.AddListener(RemoveItemFromInventory);
        inventoryEvent.OnItemSelected.AddListener(ShowItemDescription);

        //Gear events
        inventoryEvent.OnGearEquiped.AddListener(UpdateStatusPanel);
        inventoryEvent.OnGearUnEquiped.AddListener(UnEquipGear);
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

    void UpdateStatusPanel(ItemData itemData)
    {
        GearData gear = itemData as GearData;
        foreach (StatusGear status in gear.statusGears)
        {
            switch (status.enumStatus)
            {
                case EnumGearStatus.MaxHealth:
                    int.TryParse(healthValueText.text, out int currentHealth);
                    currentHealth += status.value;
                    healthValueText.text = $"{currentHealth}";
                    break;
                case EnumGearStatus.ATK:
                    int.TryParse(atkValueText.text, out int currentAtk);
                    currentAtk += status.value;
                    atkValueText.text = $"{currentAtk}";
                    break;
                case EnumGearStatus.DEF:
                    int.TryParse(defValueText.text, out int currentDef);
                    currentDef += status.value;
                    defValueText.text = $"{currentDef}";
                    break;
                default:
                    break;
            }
        }
    }

    void UnEquipGear(ItemData itemData)
    {
        GearData gear = itemData as GearData;
        foreach (StatusGear status in gear.statusGears)
        {
            switch (status.enumStatus)
            {
                case EnumGearStatus.MaxHealth:
                    int.TryParse(healthValueText.text, out int currentHealth);
                    currentHealth -= status.value;
                    healthValueText.text = $"{currentHealth}";
                    break;
                case EnumGearStatus.ATK:
                    int.TryParse(atkValueText.text, out int currentAtk);
                    currentAtk -= status.value;
                    atkValueText.text = $"{currentAtk}";
                    break;
                case EnumGearStatus.DEF:
                    int.TryParse(defValueText.text, out int currentDef);
                    currentDef -= status.value;
                    defValueText.text = $"{currentDef}";
                    break;
                default:
                    break;
            }
        }
    }

    void AddItemToInventory(ItemData itemData)
    {
        foreach (Slot slot in inventorySlots)
        {
            DraggableItem item = slot.transform.GetChild(0).GetComponent<DraggableItem>();
            if (item.itemData == null)
            {
                item.itemData = itemData;
                item.image.sprite = itemData.itemIcon;
                //Add Item to inventory model by controller
                return;
            }
        }
    }

    void RemoveItemFromInventory(ItemData itemData)
    {
        foreach (Slot slot in inventorySlots)
        {
            DraggableItem item = slot.transform.GetChild(0).GetComponent<DraggableItem>();
            if (item.itemData == itemData)
            {
                item.itemData = null;
                item.image.sprite = null;
                //Remove Item from inventory model by controller
                return;
            }
        }
    }

}
