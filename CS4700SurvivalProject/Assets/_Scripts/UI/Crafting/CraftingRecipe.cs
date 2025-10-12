using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    public Item[] requiredPattern = new Item[8];
    public Item resultItem;
}
