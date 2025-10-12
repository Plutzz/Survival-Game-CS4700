using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public bool isResultSlot = false;
    public void OnDrop(PointerEventData eventData)
    {
        if (isResultSlot) 
        {
            Debug.Log("Cannot drop items on a result slot.");
            return; // ignore any drops
        }
        if (!OpenInventory.InventoryOpen)
            return;

        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        InventoryItem droppedItem = dropped.GetComponent<InventoryItem>();
        if (droppedItem == null) return;

        // Find the current item in this slot (ignore non-item children like cursor graphics)
        InventoryItem currentItem = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            InventoryItem item = transform.GetChild(i).GetComponent<InventoryItem>();
            if (item != null)
            {
                currentItem = item;
                break;
            }
        }

        if (currentItem == null)
        {
            // Slot empty: just assign
            droppedItem.parentAfterDrag = transform;
            droppedItem.transform.SetParent(transform);
            droppedItem.transform.localPosition = Vector3.zero;
        }
        else
        {
            // Slot occupied: swap items

            // Store old parent of dropped item
            Transform oldParent = droppedItem.parentAfterDrag;

            // Put dropped item into this slot
            droppedItem.parentAfterDrag = transform;
            droppedItem.transform.SetParent(transform);
            droppedItem.transform.localPosition = Vector3.zero;

            // Move current item to the old slot of dropped item
            if (oldParent != null)
            {
                currentItem.parentAfterDrag = oldParent;
                currentItem.transform.SetParent(oldParent);
                currentItem.transform.localPosition = Vector3.zero;
            }
            else
            {
                // If dropped item didn't have a valid old parent, just leave it in place
                Debug.LogWarning("Dropped item had no valid parent to swap with.");
            }
        }
    }
}
