using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : NetworkBehaviour, IDamageable
{

    public Action OnTakeDamageServerRpc;
    
    [field: SerializeField] public Rigidbody2D rb { get; private set; }
    [field: SerializeField] public Animator animator { get; private set; }

    [SerializeField] protected NetworkVariable<int> health = new NetworkVariable<int>(100);
    private NetworkVariable<bool> isAlive = new NetworkVariable<bool>(true);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!isAlive.Value)
        {
            Destroy(gameObject);
            return;
        }
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
        else
        {
            OnTakeDamageServerRpc?.Invoke();
        }
    }
    
    public virtual void Die()
    {
        isAlive.Value = false;
        Invoke(nameof(Destroy), 3f);
    }
    
    private void Destroy()
    {
        NetworkObject.Despawn();
    }
    

}
