using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DamageBox : NetworkBehaviour
{
    [SerializeField] private int damage;
    private List<IDamageable> hasBeenDamaged = new List<IDamageable>();
    private NetworkObject ownerNetworkObject;
    
    private void Awake()
    {
        ownerNetworkObject = GetComponentInParent<NetworkObject>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        
        gameObject.SetActive(false);
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Owner of object: " + ownerNetworkObject.IsOwner);
        
        if (!ownerNetworkObject.IsOwner) return;
        
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && !hasBeenDamaged.Contains(damageable))
        {
            Debug.Log("HIT OBJECT");
            hasBeenDamaged.Add(damageable);
            damageable.TakeDamage(damage);
        }
    }

    public void ClearHasBeenDamaged()
    {
        hasBeenDamaged.Clear();
    }
}
