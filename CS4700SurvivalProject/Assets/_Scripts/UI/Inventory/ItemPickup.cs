using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemPickup : NetworkBehaviour
{
    public Item itemDefinition; // Assign the ScriptableObject
    public string instanceId;
    public int count = 1;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (string.IsNullOrEmpty(instanceId))
            instanceId = System.Guid.NewGuid().ToString();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // If the definition was pre-assigned on the prefab, apply its sprite.
        if (itemDefinition != null)
            ApplyDefinition();
    }

    public void Initialize(Item newItem, int newCount = 1)
    {
        itemDefinition = newItem;
        count = newCount;
        ApplyDefinition(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player touched " + itemDefinition.name);
            AttemptPickup();
        }
    }

    public bool AttemptPickup()
    {
        if (itemDefinition == null)
            return false;

        // Local / single-player behaviour: use InventoryManager
        if (InventoryManager.Instance != null)
        {
            bool added = InventoryManager.Instance.AddItem(itemDefinition);
            if (added)
            {
                Destroy(gameObject);
                return true;
            }
        }
        return false;
    }
    // Call this after assigning itemDefinition at runtime to update visuals.
    public void ApplyDefinition()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && itemDefinition != null)
        {
            spriteRenderer.sprite = itemDefinition.image;
        }
    }
}
