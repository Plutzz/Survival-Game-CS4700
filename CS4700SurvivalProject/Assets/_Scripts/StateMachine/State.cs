using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Base class that all states inherit from
/// </summary>

[System.Serializable]
public class State<TContext>
{
    public TContext Context { get; private set; }
    public bool IsComplete { get; protected set; }

    protected float StateUptime = 0;

    public StateMachine<TContext> StateMachine;
    public void SetState(State<TContext> newState, bool forceReset = false)
    {
        StateMachine.SetState(newState, forceReset);
    }

    public void Init(TContext context)
    {
        Context = context;
    }
    
    /// <summary>
    /// Setup state, e.g. starting animations.
    /// Consider this the "Start" method of this state.
    /// </summary>
    public virtual void EnterState() { }

    /// <summary>
    /// State-Cleanup.
    /// </summary>
    public virtual void ExitState() { StateMachine?.CurrentState?.ExitState(); ResetValues(); }

    /// <summary>
    /// This method is called once every frame while this state is active.
    /// Consider this the "Update" method of this state.
    /// </summary>
    public virtual void UpdateState() { StateMachine?.CurrentState?.UpdateState(); HandleTimer(); }

    /// <summary>
    /// This method is called once every physics frame while this state is active.
    /// Consider this the "FixedUpdate" method of this state.
    /// </summary>
    public virtual void FixedUpdateState() { StateMachine?.CurrentState?.FixedUpdateState(); }
    

    /// <summary>
    /// This method is called during ExitLogic().
    /// Use this method to reset or null out values during state cleanup.
    /// </summary>
    public virtual void ResetValues() { StateUptime = 0f; IsComplete = false; }
    protected void HandleTimer()
    {
        StateUptime += Time.deltaTime;
    }

}