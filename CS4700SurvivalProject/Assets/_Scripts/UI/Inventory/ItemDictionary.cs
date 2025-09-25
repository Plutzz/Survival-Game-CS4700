using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    private Dictionary<int, Item> itemDictionary;

    private void Awake()
    {
        itemDictionary = new Dictionary<int, Item>();

        // Auto Increment IDs
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].ID = i + 1;
            }
        }
        foreach (Item item in itemPrefabs)
        {
            itemDictionary[item.ID] = item;
        }
    }

    public Item GetItemPrefab(int itemID)
    {
        itemDictionary.TryGetValue(itemID, out Item prefab);
        if (prefab == null)
        {
            Debug.LogWarning($"Item with ID {itemID} not found in dictionary");
        }
        return prefab;
    }
}
