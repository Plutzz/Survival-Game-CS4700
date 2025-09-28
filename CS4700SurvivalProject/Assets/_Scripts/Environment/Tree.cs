using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Tree : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 10;
    private int health;
    [SerializeField] private MMF_Player _mmfPlayer;
    [SerializeField] private GameObject graphics;
    [SerializeField] private Collider2D collider;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private AudioSource deathAudio;

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
        graphics.SetActive(false);
        deathParticles.GetComponent<Renderer>().sortingOrder = graphics.GetComponent<Renderer>().sortingOrder + 1;
        deathParticles.Play();
        collider.enabled = false;
        deathAudio.Play();
        Destroy(gameObject, 3f);
    }
}
