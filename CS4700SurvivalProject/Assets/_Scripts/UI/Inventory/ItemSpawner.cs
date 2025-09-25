using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPickupPrefab;
    [Tooltip("Optional list of Item definitions you can pick from in the Button inspector")]
    public Item[] items;

    // Spawn at a world position
    public GameObject SpawnItem(Item itemDef, Vector3 worldPosition, int count = 1)
    {
        if (itemPickupPrefab == null || itemDef == null) return null;

        GameObject go = Instantiate(itemPickupPrefab, worldPosition, Quaternion.identity);
        ItemPickup pickup = go.GetComponent<ItemPickup>();
        if (pickup != null)
        {
            pickup.itemDefinition = itemDef;
            pickup.count = count;
            // Ensure a unique instance id
            if (string.IsNullOrEmpty(pickup.instanceId))
                pickup.instanceId = System.Guid.NewGuid().ToString();
            // Apply visuals from the definition (sprite etc)
            pickup.ApplyDefinition();
        }
        return go;
    }

    public GameObject SpawnItemAtSelf(Item itemDef, int count = 1)
    {
        return SpawnItem(itemDef, transform.position, count);
    }

    // Inspector-friendly wrapper: spawn the item at `items[index]` at this transform.
    // Unity Button can call this because it accepts an int parameter.
    public void SpawnItemAtSelfByIndex(int index)
    {
        if (items == null || index < 0 || index >= items.Length)
            return;
        SpawnItemAtSelf(items[index], 1);
    }

    // Inspector-friendly no-argument wrapper that spawns the first item in `items`.
    public void SpawnFirstItemAtSelf()
    {
        if (items == null || items.Length == 0) return;
        SpawnItemAtSelf(items[0], 1);
    }
}
