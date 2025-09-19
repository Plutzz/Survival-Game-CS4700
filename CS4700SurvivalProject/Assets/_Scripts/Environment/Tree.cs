using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Tree : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 10;
    private int health;
    [SerializeField] private MMF_Player _mmfPlayer;

    public void Start()
    {
        health = maxHealth;
    }
            
    public void TakeDamage(int damage)
    {
        health -= damage;
        _mmfPlayer.PlayFeedbacks();
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
