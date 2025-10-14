using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Enemy : StateMachineCore, IDamageable
{
    [SerializeField] protected NetworkVariable<int> health = new NetworkVariable<int>(100);
    
    protected void Update()
    {
        if (!IsServer) return;
        DoAI();
    }

    /// <summary>
    /// This is the method you should override and write the enemy AI in (runs in update on server only)
    /// </summary>
    public virtual void DoAI()
    {
        
    }

    public void TakeDamage(int damageTaken)
    {
        TakeDamageServerRpc(damageTaken);
    }
    

    [ServerRpc(RequireOwnership = false)]
    protected virtual void TakeDamageServerRpc(int damage)
    {
        health.Value -= damage;
        Debug.Log("Enemy Take Damage: " + health.Value);
        if (health.Value <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        
    }
}
