using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CraftingSlot : MonoBehaviour, IDropHandler
{
    [HideInInspector] public InventoryItem currentItem;
    public event Action OnSlotUpdated;

    // Public helper so other components (like InventoryItem) can notify that
    // the slot's visible contents changed (for example stack count changed).
    public void NotifySlotUpdated()
    {
        OnSlotUpdated?.Invoke();
    }

    private void Update()
    {
        // Detect if an item was removed manually (dragged out or destroyed)
        if (currentItem != null && currentItem.transform.parent != transform)
        {
            Debug.Log($"[{name}] Item {currentItem.item.name} removed from slot.");
            currentItem = null;
            OnSlotUpdated?.Invoke();
        }
        // Detect if an item was manually placed (shouldn’t happen normally but safe)
        else if (currentItem == null && transform.childCount > 0)
        {
            InventoryItem foundItem = GetComponentInChildren<InventoryItem>();
            if (foundItem != null)
            {
                currentItem = foundItem;
                Debug.Log($"[{name}] Item {foundItem.item.name} added to slot.");
                OnSlotUpdated?.Invoke();
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        InventoryItem droppedItem = dropped.GetComponent<InventoryItem>();
        if (droppedItem == null) return;

        // Notify old crafting slot (if applicable)
        if (droppedItem.parentAfterDrag != null &&
            droppedItem.parentAfterDrag.TryGetComponent(out CraftingSlot oldSlot))
        {
            oldSlot.currentItem = null;
            Debug.Log($"[{oldSlot.name}] Item removed (moved to {name}).");
            oldSlot.OnSlotUpdated?.Invoke();
        }

        // Assign the new item
        currentItem = droppedItem;
        Debug.Log($"[{name}] Item {droppedItem.item.name} dropped in.");
        OnSlotUpdated?.Invoke();
    }

    public ItemSO GetItem()
    {
        return currentItem != null ? currentItem.item : null;
    }
}
