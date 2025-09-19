using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItems(int id)
    {
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if (result)
        {
            Debug.Log("Item Added");
        }
        else
        {
            Debug.Log("Inventory Full!");
        }
    }

    public void GetSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectedItem(false);
        if (receivedItem != null)
        {
            Debug.Log("Recieved Item: " + receivedItem);
        }
        else
        {
            Debug.Log("No Item Received");
        }
    }

    public void UseSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectedItem(true);
        if (receivedItem != null)
        {
            Debug.Log("Used Item: " + receivedItem);
        }
        else
        {
            Debug.Log("No Item Used");
        }
    }
}
