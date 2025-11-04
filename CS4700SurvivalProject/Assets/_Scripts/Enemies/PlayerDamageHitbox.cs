using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHitbox : MonoBehaviour
{
    [SerializeField] private int damage;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered Hitbox of" + other.gameObject.name);
        if (other.TryGetComponent(out PlayerHealth player))
        {
            player.TakeDamage(damage);
        }
    }
}
