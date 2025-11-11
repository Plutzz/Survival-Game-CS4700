using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/Speed Buff")]
public class SpeedBuff : ScriptableObject, IItemEffect
{
    public float duration = 5f;
    public float speedMultiplier = 1.5f;

    public void Apply(Player player)
    {
        Debug.Log("Speed Buff Applied");
        // Apply Speed Buff
    }
}
