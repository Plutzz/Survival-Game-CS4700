using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Improved drag behavior: move the item to the parent Canvas while dragging,
// restore parent and raycast state on end or disable, and clear EventSystem
// selection so the UI doesn't get stuck in a pressed/selected state.
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{

    [Header("UI")]
    public Image image;
    public Text countText;
    public GameObject itemDropPrefab;
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    public ResultSlot parentResultSlot;
    
    
    // Local guard so OnDrag/OnEndDrag only act when a drag was allowed to start.
    bool draggingAllowed = false;

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        if (image != null && newItem != null)
            image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        if (countText == null) return;
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Only allow dragging when the inventory UI is open
        if (!OpenInventory.InventoryOpen)
        {
            draggingAllowed = false;
            return;
        }
        draggingAllowed = true;

        parentAfterDrag = transform.parent;

        // Move to top-level canvas during drag so the item renders above other UI.
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
            transform.SetParent(canvas.transform);
        else
            transform.SetParent(transform.root);

        transform.SetAsLastSibling();
        if (image != null)
            image.raycastTarget = false;
        Debug.Log("begin drag");
        if (parentResultSlot != null)
{
        InventoryItem claimedItem;
        if (parentResultSlot.ClaimResult(out claimedItem))
        {
            parentAfterDrag = claimedItem.transform.parent; // set parent for snapping back
            // continue drag with the item
        }
    }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!draggingAllowed) return;
        // Use the eventData position which is correct for UI events.
        transform.position = eventData.position;
        Debug.Log("dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!draggingAllowed) return;
            bool removedFromInventory = false;

        if (!IsWithinInventory(eventData.position))
            removedFromInventory = DropItem();

        if (!removedFromInventory && parentAfterDrag != null)
        {
            transform.SetParent(parentAfterDrag);
            transform.localPosition = Vector3.zero; // snap back
        }
    
        if (image != null)
            image.raycastTarget = true;
        Debug.Log("end drag");

        // Clear any selected object so the EventSystem doesn't stay in a pressed/selected state.
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        draggingAllowed = false;
    }

    void OnDisable()
    {
        // Ensure we don't leave raycast disabled if the object is disabled unexpectedly while dragging.
        if (image != null)
            image.raycastTarget = true;
        // Try to restore parent if it was left detached.
        if (parentAfterDrag != null && transform.parent != parentAfterDrag)
            transform.SetParent(parentAfterDrag);
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
        draggingAllowed = false;
    }

    // Also respond to pointer-up so we reliably clear drag state even if EndDrag isn't called.
    public void OnPointerUp(PointerEventData eventData)
    {
        // Ensure we run end-drag cleanup in case the event system didn't call OnEndDrag.
        if (draggingAllowed)
            OnEndDrag(eventData);
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        if (parentAfterDrag == null) return false;
        RectTransform parentRect = parentAfterDrag.GetComponent<RectTransform>();
        if (parentRect == null) return false;
        
        return RectTransformUtility.RectangleContainsScreenPoint(parentRect, mousePosition);
    }


    bool DropItem()
    {
        if (itemDropPrefab == null || item == null) return false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;

        // Drop in front of player
        Vector3 dropPosition = player.transform.position + player.transform.forward * 2f;
        dropPosition.y += 1f; // Slightly above ground to avoid clipping

        // Instantiate the item drop prefab
        GameObject drop = Instantiate(itemDropPrefab, dropPosition, Quaternion.identity);

        // Pass item data to the pickup
        ItemPickup pickup = drop.GetComponent<ItemPickup>();
        if (pickup != null)
        {
            if (item.stackable && count > 1)
            {
                pickup.Initialize(item, 1);
                count--;
                RefreshCount();
                return false;
            }
            else
            {
                pickup.Initialize(item, count);
                Destroy(gameObject);
                return true;
            }
        }
        return false;
    }
}
