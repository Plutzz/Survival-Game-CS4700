using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/CraftingRecipe")]
public class CraftingRecipeSO : ScriptableObject
{
    public ItemSO[] requiredPattern = new ItemSO[8];
    public ItemSO resultItem;
}
