using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class Cow : Enemy
{
    [SerializeField] private MMF_Player hitFeedback;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private AudioSource deathAudio;

    [SerializeField] private GameObject graphics;
    
    [field: Header("State Machine")]
    public StateMachine<Enemy> StateMachine { get; private set; }
    [field: SerializeField] public EnemyWander MoveState { get; private set; } = new();
    [field: SerializeField] public EnemyIdle IdleState { get; private set; } = new();
    
    


    private void Start()
    {
        InitializeStateMachine();
        StateMachine.SetState(MoveState);
        OnTakeDamageServerRpc += PlayDamageFeedbacksClientRpc;
    }

    private void Update()
    {
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
        HandleTransitions();
    }

    [ClientRpc]
    private void PlayDamageFeedbacksClientRpc()
    {
        hitFeedback.PlayFeedbacks();
    }

    public override void Die()
    {
        base.Die();
        PlayDeathFeedbacksClientRpc();
    }
    
    [ClientRpc]
    private void PlayDeathFeedbacksClientRpc()
    {
        graphics.SetActive(false);
        deathParticles.GetComponent<Renderer>().sortingOrder = graphics.GetComponent<Renderer>().sortingOrder + 1;
        deathParticles.Play();
        GetComponent<Collider2D>().enabled = false;
        deathAudio.Play();
    }

    private void HandleTransitions()
    {
        if (StateMachine.CurrentState == MoveState && StateMachine.CurrentState.IsComplete)
        {
            StateMachine.SetState(IdleState);
        }
        else if (StateMachine.CurrentState == IdleState && StateMachine.CurrentState.IsComplete)
        {
            StateMachine.SetState(MoveState);
        }
    }

    private void InitializeStateMachine()
    {
        StateMachine = new StateMachine<Enemy>(this);
        MoveState.Init(this);
        IdleState.Init(this);
    }
    
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            string stateList = "";

            foreach (var state in StateMachine.GetActiveStateBranch())
            {
                stateList += state + " >";
            }
            
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.red;
            style.fontSize = 40;
            Handles.Label(transform.position + Vector3.up, stateList, style);
        }
#endif
    }


}
