using System;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.gameObject.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                if(!playerInventory.isInventoryFull)
                {
                    playerInventory.AddItemToInventory(itemData);
                    Destroy(gameObject);
                }
            }
        }
    }
}