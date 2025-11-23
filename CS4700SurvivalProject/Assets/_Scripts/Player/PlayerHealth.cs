using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private int maxHealth;
    private NetworkVariable<int> health = new NetworkVariable<int>(100);
    
    public void TakeDamage(int damage)
    {
        if(!IsOwner) return;
        health.Value -= damage;
        AudioManager.Instance.PlaySound(AudioManager.Sounds.PlayerHurt);
        UIManager.Instance.SetHealthBar(health.Value, maxHealth);
    }

    public void Heal(int heal)
    {
        if(!IsOwner) return;
        health.Value += heal;
        AudioManager.Instance.PlaySound(AudioManager.Sounds.PlayerHeal);
        UIManager.Instance.SetHealthBar(health.Value, maxHealth);
    }
}
