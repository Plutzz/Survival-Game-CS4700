using UnityEngine;
using System.Text;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private CraftingSlot[] slots = new CraftingSlot[8];
    [SerializeField] private CraftingRecipe[] recipes;
    [SerializeField] private ResultSlot resultSlot;
    private void Awake()
    {
        foreach (var slot in slots)
        {
            if (slot != null)
                slot.OnSlotUpdated += CheckRecipe;
        }

        if (resultSlot != null)
            resultSlot.OnResultTaken += ConsumeIngredients;
    }
    
    private void Start()
    {
        CheckRecipe();
    }

    public Item[] GetItemPattern()
    {
        Item[] pattern = new Item[slots.Length];
        for (int i = 0; i < slots.Length; i++)
            pattern[i] = slots[i].GetItem();
        return pattern;
    }
    int count = 0;
    private void CheckRecipe()
    {
        Item[] currentPattern = GetItemPattern();

        foreach (var recipe in recipes)
        {
            if (recipe == null) continue;
            bool match = true;

            for (int i = 0; i < currentPattern.Length; i++)
            {
                if (recipe.requiredPattern[i] != currentPattern[i])
                {
                    match = false;
                    break;
                }
            }
            if (match)
            {
                // Only set result if resultSlot is empty
                if (resultSlot.currentItem == null)
                    resultSlot.SetItem(recipe.resultItem);
                Debug.Log("Item added: " + count);
                count++;
                if (count == 1)
                {
                    resultSlot.itemClaimed = false;
                    count = 0;
                }
                return;
            }
        }

        // No matching recipe
        resultSlot.ClearResultVisual();
    }

    private void ConsumeIngredients()
    {
        Debug.Log("ConsumeIngredients Called");

        foreach (var slot in slots)
        {
            if (slot != null && slot.currentItem != null)
            {
                InventoryItem itemInSlot = slot.currentItem;

                if (itemInSlot.count > 1)
                {
                    // Reduce stack by 1
                    itemInSlot.count--;
                    itemInSlot.RefreshCount();
                    Debug.Log($"[{slot.name}] Consuming 1 of {itemInSlot.item.name}, remaining: {itemInSlot.count}");
                }
                else
                {
                    // Only one left, destroy the item
                    Destroy(itemInSlot.gameObject);
                    slot.currentItem = null;
                    Debug.Log($"[{slot.name}] Consuming last {itemInSlot.item.name}");
                }
            }
        }
    }

    
    // public void PrintPattern()
    // {
    //     Item[] pattern = GetItemPattern();

    //     StringBuilder sb = new StringBuilder("Crafting Pattern: [");
    //     for (int i = 0; i < pattern.Length; i++)
    //     {
    //         string name = pattern[i] ? pattern[i].name : "Empty";
    //         sb.Append(name);
    //         if (i < pattern.Length - 1)
    //             sb.Append(", ");
    //     }
    //     sb.Append("]");

    //     Debug.Log(sb.ToString());
    // }
}
