using UnityEngine;
using System.Text;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private CraftingSlot[] slots = new CraftingSlot[8];
    [SerializeField] private CraftingRecipeSO[] recipes;
    [SerializeField] private ResultSlot resultSlot;
    private void Awake()
    {
        foreach (var slot in slots)
        {
            if (slot != null)
                slot.OnSlotUpdated += CheckRecipe;
        }

        if (resultSlot != null)
        {
            // When the result is claimed, consume ingredients
            resultSlot.OnResultTaken += ConsumeIngredients;
        }
    }

    private void Start()
    {
        CheckRecipe();
    }

    public ItemSO[] GetItemPattern()
    {
        ItemSO[] pattern = new ItemSO[slots.Length];
        for (int i = 0; i < slots.Length; i++)
            pattern[i] = slots[i].GetItem();
        return pattern;
    }
    int count = 0;
    private void CheckRecipe()
    {
        ItemSO[] currentPattern = GetItemPattern();
        PrintPattern();

        foreach (var recipe in recipes)
        {
            if (recipe == null) continue;
            Debug.Log("Checking recipe for: " + recipe.resultItem.name);
            bool match = true;

            for (int i = 0; i < currentPattern.Length; i++)
            {
                if (recipe.requiredPattern[i] != currentPattern[i])
                {
                    match = false;
                    Debug.Log("Pattern mismatch at index " + i);
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

        // After consuming ingredients (player took the result), re-evaluate recipes once.
        // This avoids recursive event loops while ensuring the crafting result updates
        // immediately after a take.
        CheckRecipe();
    }

    public void PrintPattern()
    {
        ItemSO[] pattern = GetItemPattern();

        StringBuilder sb = new StringBuilder("Crafting Pattern: [");
        for (int i = 0; i < pattern.Length; i++)
        {
            string name = pattern[i] ? pattern[i].name : "Empty";
            sb.Append(name);
            if (i < pattern.Length - 1)
                sb.Append(", ");
        }
        sb.Append("]");

        Debug.Log(sb.ToString());
    }
}
