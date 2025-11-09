using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup : NetworkBehaviour
{
    public ItemSO itemDefinition; // Assign the ScriptableObject
    public int count = 1;
    SpriteRenderer spriteRenderer;
    private float timeBeforePickup = 0.25f;
    private float timer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // If the definition was pre-assigned on the prefab, apply its sprite.
        if (itemDefinition != null)
            ApplyDefinition(itemDefinition);
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    public void Initialize(ItemSO newItem, int newCount = 1)
    {
        itemDefinition = newItem;
        count = newCount;
        ApplyDefinition(itemDefinition); 
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AttemptPickup();
        }
    }

    public bool AttemptPickup()
    {
        if (itemDefinition == null || timer < timeBeforePickup)
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
    public void ApplyDefinition(ItemSO newDefinition)
    {
        itemDefinition = newDefinition;
        if (itemDefinition != null)
        {
            spriteRenderer.sprite = itemDefinition.worldSprite;
            spriteRenderer.transform.localScale = itemDefinition.worldScale * Vector3.one;
        }
    }
}
