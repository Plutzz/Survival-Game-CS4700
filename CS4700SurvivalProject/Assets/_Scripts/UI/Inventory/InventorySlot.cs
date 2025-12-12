using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool isResultSlot = false;
    public void OnDrop(PointerEventData eventData)
    {
        if (isResultSlot)
        {
            Debug.Log("Cannot drop items on a result slot.");
            return; // ignore any drops
        }
        if (GameManager.Instance.CurrentGameState != GameState.Inventory)
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

        // Get InventoryManager to help find empty slots
        InventoryManager inventoryManager = InventoryManager.Instance;
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager not found!");
            return;
        }

        // Handle result items differently to avoid invalid swaps with result slot
        bool isFromResultSlot = droppedItem.parentResultSlot != null;

        if (currentItem == null)
        {
            // Slot empty: just assign
            droppedItem.parentAfterDrag = transform;
            droppedItem.transform.SetParent(transform, false);
            droppedItem.transform.localPosition = Vector3.zero;
        }
        else if (currentItem.item == droppedItem.item && currentItem.item != null && currentItem.item.stackable)
        {
            // Merge stacks: add count to existing and destroy the dragged UI
            currentItem.count += droppedItem.count;
            currentItem.RefreshCount();

            // Stop drag on the source and destroy the dragged UI element (it has been merged)
            droppedItem.StopDragging();
            Destroy(dropped);
        }
        else if (isFromResultSlot)
        {
            // If item is from result slot and target is occupied, find an empty inventory slot
            bool found = false;
            foreach (InventorySlot slot in inventoryManager.inventorySlots)
            {
                if (slot.isResultSlot) continue; // Skip result slots

                InventoryItem existingItem = null;
                for (int i = 0; i < slot.transform.childCount; i++)
                {
                    existingItem = slot.transform.GetChild(i).GetComponent<InventoryItem>();
                    if (existingItem != null) break;
                }

                if (existingItem == null)
                {
                    // Found empty slot - place item here
                    droppedItem.parentAfterDrag = slot.transform;
                    droppedItem.transform.SetParent(slot.transform, false);
                    droppedItem.transform.localPosition = Vector3.zero;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                // No empty slots - snap back to original position
                Debug.Log("No empty slots available for result item");
                if (droppedItem.parentAfterDrag != null)
                {
                    droppedItem.transform.SetParent(droppedItem.parentAfterDrag, false);
                    droppedItem.transform.localPosition = Vector3.zero;
                }
            }
        }
        else
        {
            // Normal swap for non-result items
            Transform oldParent = droppedItem.parentAfterDrag;

            // Put dropped item into this slot
            droppedItem.parentAfterDrag = transform;
            droppedItem.transform.SetParent(transform, false);
            droppedItem.transform.localPosition = Vector3.zero;

            // Move current item to the old slot of dropped item
            if (oldParent != null)
            {
                currentItem.parentAfterDrag = oldParent;
                currentItem.transform.SetParent(oldParent, false);
                currentItem.transform.localPosition = Vector3.zero;
            }
            else
            {
                Debug.LogWarning("Dropped item had no valid parent to swap with.");
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Enter");
        InventoryManager.Instance.ShowTooltip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse Exit");
        InventoryManager.Instance.HideTooltip(this);
    }
}