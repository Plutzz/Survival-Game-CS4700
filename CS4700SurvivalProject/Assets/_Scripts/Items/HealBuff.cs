using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/Heal Buff")]
public class HealBuff : ScriptableObject, IItemEffect
{
    public int numTicks = 1;
    public float delayBetweenTicks = 0.5f;
    public float healAmount = 10f;

    public void Apply(Player player)
    {
        Debug.Log("Heal Buff Applied");
        // Apply Heal Buff
    }
}
