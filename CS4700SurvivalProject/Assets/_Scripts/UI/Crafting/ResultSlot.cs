using UnityEngine;
using UnityEngine.UI;

public class ResultSlot : MonoBehaviour
{
    [HideInInspector] public Item currentItem;
    [SerializeField] private InventorySlot resultSlot;
    public GameObject InventoryItemPrefab;
    public System.Action OnResultTaken;
    // Invoked when the result visual or stored result changes (set/cleared/claimed)
    public System.Action OnResultUpdated;
    private InventoryItem spawnedResultItem;
    [HideInInspector] public bool itemClaimed = false;

    public bool ClaimResult(out InventoryItem item)
    {
        if (currentItem == null || spawnedResultItem == null)
        {
            item = null;
            return false;
        }

        item = spawnedResultItem;
        currentItem = null;
        spawnedResultItem = null;
        itemClaimed = true;   // Mark as claimed
        // First notify that the result was taken so ingredients can be consumed
        OnResultTaken?.Invoke();
        // Then notify listeners that the result slot state changed
        OnResultUpdated?.Invoke();
        Debug.Log("Result claimed for drag, ingredients should be consumed.");
        return true;
    }

    public void SetItem(Item item)
    {
        // Don't recreate the same item if it's already claimed
        if ((currentItem == item && spawnedResultItem != null) || itemClaimed)
            return;

        SpawnNewItem(item);
        currentItem = item;
        itemClaimed = false;  // Reset when spawning a new item
        // Notify listeners that the result slot has been updated
        OnResultUpdated?.Invoke();
    }

    public void ClearResultVisual()
    {
        if (spawnedResultItem != null)
        {
            Destroy(spawnedResultItem.gameObject);
            spawnedResultItem = null;
        }

        currentItem = null;
        // Notify listeners that the result slot has been cleared
        OnResultUpdated?.Invoke();
    }

    private void SpawnNewItem(Item item)
    {
        
        if (item == null || InventoryItemPrefab == null) return;

        GameObject newItemGo = Instantiate(InventoryItemPrefab, resultSlot.transform);
        spawnedResultItem = newItemGo.GetComponent<InventoryItem>();
        spawnedResultItem.InitializeItem(item);
        spawnedResultItem.count = 1;
        spawnedResultItem.RefreshCount();
        spawnedResultItem.parentResultSlot = this;
    }
}
