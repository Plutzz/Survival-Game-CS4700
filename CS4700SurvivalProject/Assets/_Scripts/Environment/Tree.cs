using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Tree : NetworkBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 10;
    private NetworkVariable<int> health = new NetworkVariable<int>(100);
    private NetworkVariable<bool> isAlive = new NetworkVariable<bool>(true);
    [SerializeField] private MMF_Player damageFeedback, timeFeedback;
    [SerializeField] private GameObject graphics;
    [SerializeField] private Collider2D collider;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private AudioSource deathAudio;


    public override void OnNetworkSpawn()
    {
        if (!isAlive.Value)
        {
            gameObject.SetActive(false);
            return;
        }
        
        if (!IsServer) return;
        health.Value = maxHealth;
    }

    private void Update()
    {

    }
    
    public void TakeDamage(int damage)
    {
        Debug.Log("Tree Taking Damage: " + IsSpawned);
        TakeDamageServerRpc(damage);
        timeFeedback.PlayFeedbacks();
    }
    
    [ServerRpc(RequireOwnership = false)]
    protected virtual void TakeDamageServerRpc(int damage)
    {
        health.Value -= damage;
        PlayDamageFeedbacksClientRpc();
        if (health.Value <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        PlayDeathFeedbacksClientRpc();
        isAlive.Value = false;
        Invoke(nameof(Destroy), 3f);
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
    
    [ClientRpc]
    private void PlayDeathFeedbacksClientRpc()
    {
        graphics.SetActive(false);
        deathParticles.GetComponent<Renderer>().sortingOrder = graphics.GetComponent<Renderer>().sortingOrder + 1;
        deathParticles.Play();
        collider.enabled = false;
        deathAudio.Play();
    }

    [ClientRpc]
    private void PlayDamageFeedbacksClientRpc()
    {
        damageFeedback.PlayFeedbacks();
    }
}
