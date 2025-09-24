using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    [SerializeField] private int damage;
    private List<IDamageable> hasBeenDamaged = new List<IDamageable>();
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && !hasBeenDamaged.Contains(damageable))
        {
            hasBeenDamaged.Add(damageable);
            damageable.TakeDamage(damage);
        }
    }

    public void ClearHasBeenDamaged()
    {
        hasBeenDamaged.Clear();
    }
}
