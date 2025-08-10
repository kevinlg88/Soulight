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
    [SerializeField] List<Slot> statusSlots = new List<Slot>();

    [Header("Inventory Panel")]
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] List<Slot> inventorySlots = new List<Slot>();
    [SerializeField] Button trash;


    private InventoryEvent _inventoryEvent;
    private InventoryController _inventoryController;
    private ItemData selectedItemData;

    [Inject]
    public void Construct(InventoryEvent inventoryEvent, InventoryController inventoryController)
    {
        _inventoryEvent = inventoryEvent;
        _inventoryController = inventoryController;
        inventoryController.SetInventoryEvent(inventoryEvent);
        //Inventory events
        inventoryEvent.OnItemAdded.AddListener(AddItemToInventory);
        inventoryEvent.OnItemRemoved.AddListener(RemoveItemFromInventory);
        inventoryEvent.OnItemSelected.AddListener(ShowItemDescription);
        inventoryEvent.OnItemUsed.AddListener(UseItem);

        //Gear events
        inventoryEvent.OnGearEquiped.AddListener(UpdateStatusPanel);
        inventoryEvent.OnGearUnEquiped.AddListener(UnEquipGear);

        trash.onClick.AddListener(DeleteItem);
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
        selectedItemData = itemData;
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

    void ConsumeItem(ItemData itemData)
    {
        ConsumableData consumable = itemData as ConsumableData;
        foreach (StatusConsumable status in consumable.status)
        {
            switch (status.enumStatus)
            {
                case EnumConsumable.IncreaseHealth:
                    int.TryParse(healthValueText.text, out int currentHealth);
                    currentHealth += status.value;
                    healthValueText.text = $"{currentHealth}";
                    break;
                case EnumConsumable.IncreaseDefense:
                    int.TryParse(defValueText.text, out int currentDef);
                    currentDef += status.value;
                    defValueText.text = $"{currentDef}";
                    break;
                case EnumConsumable.IncreaseAttack:
                    int.TryParse(atkValueText.text, out int currentAtk);
                    currentAtk += status.value;
                    atkValueText.text = $"{currentAtk}";
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
                return;
            }
        }
    }

    void RemoveItemFromInventory(ItemData itemData)
    {
        if (itemData == null) return;
        Debug.Log($"Removing item: {itemData.itemName} from inventory.");
        foreach (Slot slot in inventorySlots)
        {
            DraggableItem item = slot.transform.GetChild(0).GetComponent<DraggableItem>();
            if (item.itemData == itemData)
            {
                item.itemData = null;
                item.image.sprite = null;
                return;
            }
        }
    }

    void UseItem(ItemData itemData)
    {
        if (itemData is GearData)
        {
            foreach (var item in statusSlots)
            {
                DraggableItem draggableItem = item.transform.GetChild(0).GetComponent<DraggableItem>();
                if (draggableItem.itemData == null)
                {
                    draggableItem.itemData = itemData;
                    draggableItem.image.sprite = itemData.itemIcon;
                    draggableItem.wasEquipped = true;
                    _inventoryEvent.OnGearEquiped.Invoke(itemData);
                    Debug.Log($"Equipped {itemData.itemName} in slot {item.name}");
                    break;
                }
            }
        }
        else if (itemData is ConsumableData) ConsumeItem(itemData);
        RemoveItemFromInventory(itemData);
    }
    
    void DeleteItem()
    {
        if (selectedItemData == null) return;
        Debug.Log($"Deleting item: {selectedItemData.itemName} from inventory.");
        _inventoryController.RemoveItem(selectedItemData);
        selectedItemData = null;
        descriptionPanel.SetActive(false);
        itemNameText.text = string.Empty;
        itemDescriptionText.text = string.Empty;
        itemIconImage.sprite = null;
    }

}
