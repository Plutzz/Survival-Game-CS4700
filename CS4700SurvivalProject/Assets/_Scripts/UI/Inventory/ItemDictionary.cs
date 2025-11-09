using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<ItemSO> itemPrefabs;
    private Dictionary<int, ItemSO> itemDictionary;

    private void Awake()
    {
        itemDictionary = new Dictionary<int, ItemSO>();

        // Auto Increment IDs
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].ID = i + 1;
            }
        }
        foreach (ItemSO item in itemPrefabs)
        {
            itemDictionary[item.ID] = item;
        }
    }

    public ItemSO GetItemPrefab(int itemID)
    {
        itemDictionary.TryGetValue(itemID, out ItemSO prefab);
        if (prefab == null)
        {
            Debug.LogWarning($"Item with ID {itemID} not found in dictionary");
        }
        return prefab;
    }
}
