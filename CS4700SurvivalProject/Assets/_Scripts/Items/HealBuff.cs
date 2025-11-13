using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/Heal Buff")]
public class HealBuff : ScriptableObject, IItemEffect
{
    public int numTicks = 1;
    public float delayBetweenTicks = 0.5f;
    public int healAmount = 10;

    public void Apply(Player player)
    {
        Debug.Log("Heal Buff Applied");
        player.StartCoroutine(HealTick(player));
    }

    public IEnumerator HealTick(Player player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        for (int i = 0; i < numTicks; i++)
        {
            playerHealth.Heal(healAmount);
            yield return new WaitForSeconds(delayBetweenTicks);
        }

    }
}
