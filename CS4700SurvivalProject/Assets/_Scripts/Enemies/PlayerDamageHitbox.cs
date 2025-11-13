using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHitbox : MonoBehaviour
{
    [SerializeField] private int damage;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerHealth player))
        {
            player.TakeDamage(damage);
        }
    }
}
