using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// This is an example class for setting up a state machine
/// In Start: make sure to call SetupInstances() and set the initial state of the state machine
/// In Update: make sure to call base.Update() or stateMachine.UpdateBranch();
/// In FixedUpdate: make sure to call base.FixedUpdate() or StateMachine.FixedUpdateBranch();
/// </summary>
public abstract class StateMachineExample : MonoBehaviour
{
    public StateMachine<StateMachineExample> StateMachine;
    public Rigidbody2D rb;
    /// <summary>
    /// GameObject that is the parent to all states
    /// </summary>
    public GameObject statesParent;
    public Animator animator;
    
    /// <summary>
    /// Passes the core to all the states in the states dictionary
    /// </summary>
    public void SetupInstances()
    {
        // Set up context in all child states
    }

    protected virtual void Update()
    {
        StateMachine.Update();
    }

    protected virtual void FixedUpdate()
    { 
        StateMachine.FixedUpdate();
    }

    // public virtual void OnDrawGizmos()
    // {
    //     #if UNITY_EDITOR
    //     if (Application.isPlaying)
    //     {
    //         List<State<StateMachineExample>> states = GetActiveStateBranch();
    //
    //         GUIStyle style = new GUIStyle();
    //         style.alignment = TextAnchor.MiddleCenter;
    //         if(StateMachine.CurrentState.IsComplete)
    //         {
    //             style.normal.textColor = Color.green;
    //         }
    //         else
    //         {
    //             style.normal.textColor = Color.red;
    //         }
    //         style.fontSize = 40;
    //         UnityEditor.Handles.Label(transform.position + Vector3.up,string.Join(" > ", states), style);
    //     
    //     }
    //     #endif
    // }
    //
    // [ClientRpc]
    // public void SetStateClientRpc(string stateName, bool forceReset)
    // {
    //     Debug.Log($"Client {OwnerClientId} state changed to {allStates[stateName]}");
    //     SetState(allStates[stateName], forceReset);
    // }
    //
    // [ServerRpc(RequireOwnership = false)] 
    // public void SetStateServerRpc(string stateName, bool forceReset)
    // {
    //     SetStateClientRpc(stateName, forceReset);
    // }
    //
   
}
