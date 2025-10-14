using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// This is the core class for state machines (where all the states are initialized and logic is called)
/// In Start: make sure to call SetupInstances() and set the initial state of the state machine
/// In Update: make sure to call base.Update() or currentState.DoUpdateBranch();
/// In FixedUpdate: make sure to call base.FixedUpdate() or currentState.DoFixedUpdateBranch();
/// </summary>
public abstract class StateMachineCore : NetworkBehaviour
{
    [Header("StateMachineCore Variables")]
    public Rigidbody2D rb;
    /// <summary>
    /// GameObject that is the parent to all states in this stateMachineCore
    /// </summary>
    public GameObject statesParent;
    public Animator animator;
    public State CurrentState { get; private set; }
    public State PreviousState { get; private set; }

    [SerializeField] public SerializedDictionary<string, State> allStates;
    // This event is called when a state is changed and passes the previous state (from state) and new state (to state)
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State PreviousState;
        public State NextState;
    }
    
    /// <summary>
    /// Passes the core to all the states in the states dictionary
    /// </summary>
    public void SetupInstances()
    {
        
        State[] allChildStates;

        if (statesParent == null)
        {
            allChildStates = GetComponentsInChildren<State>();
        }
        else
        {
            allChildStates = statesParent.GetComponentsInChildren<State>();
        }

        foreach (State state in allChildStates)
        {
            state.SetCore(this);
            state.gameObject.SetActive(false);
        }
    }

    protected virtual void Update()
    {
        CurrentState.DoUpdateBranch();
    }

    protected virtual void FixedUpdate()
    { 
        CurrentState.DoFixedUpdateBranch();
    }

    public virtual void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        if (Application.isPlaying)
        {
            List<State> states = GetActiveStateBranch();
    
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            if(CurrentState.IsComplete)
            {
                style.normal.textColor = Color.green;
            }
            else
            {
                style.normal.textColor = Color.red;
            }
            style.fontSize = 40;
            UnityEditor.Handles.Label(transform.position + Vector3.up,string.Join(" > ", states), style);
        
        }
        #endif
    }

    /// <summary>
    /// Sets the state machine with a specified state
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="forceReset"</param>
    public void SetState(State newState, bool forceReset = false)
    {
        if (CurrentState != newState || forceReset)
        {
            //Debug.Log("Changing State to " + _newState);
            CurrentState?.DoExitState();
            CurrentState?.gameObject.SetActive(false);
            PreviousState = CurrentState;
            CurrentState = newState;
            CurrentState.gameObject.SetActive(true);
            CurrentState.DoEnterState();
            OnStateChanged?.Invoke(this,
                new OnStateChangedEventArgs { PreviousState = PreviousState, NextState = CurrentState });
        }

    }

    [ClientRpc]
    public void SetStateClientRpc(string stateName, bool forceReset)
    {
        Debug.Log($"Client {OwnerClientId} state changed to {allStates[stateName]}");
        SetState(allStates[stateName], forceReset);
    }
    
    [ServerRpc(RequireOwnership = false)] 
    public void SetStateServerRpc(string stateName, bool forceReset)
    {
        SetStateClientRpc(stateName, forceReset);
    }
    
    
    public List<State> GetActiveStateBranch(List<State> list = null)
    {
        if (list == null)
            list = new List<State>();

        if (CurrentState == null)
            return list;

        else
        {
            list.Add(CurrentState);
            return CurrentState.StateMachine.GetActiveStateBranch(list);
        }

    }



   
}
