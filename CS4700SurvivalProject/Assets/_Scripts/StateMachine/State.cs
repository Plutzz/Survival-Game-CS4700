using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Base class that all state scriptable objects inherit from.
/// </summary>
public class State : NetworkBehaviour
{
    protected StateMachineCore Core;

    protected Rigidbody2D Rb => Core.rb;
    protected Animator Animator => Core.animator;
    public bool IsComplete { get; protected set; }

    protected float StateUptime = 0;

    public StateMachine StateMachine;

    public StateMachine Parent;
    public State CurrentState => StateMachine.currentState;
    public void SetState(State newState, bool forceReset = false)
    {
        StateMachine.SetState(newState, forceReset);
    }

    /// <summary>
    /// Passes the StateMachineCore to this state.
    /// Can be overriden to initialize additional parameters
    /// </summary>
    /// <param name="core"></param>
    public virtual void SetCore(StateMachineCore core)
    {
        StateMachine = new StateMachine();
        Core = core;
    }
    /// <summary>
    /// Setup state, e.g. starting animations.
    /// Consider this the "Start" method of this state.
    /// </summary>
    public virtual void DoEnterState() { }

    /// <summary>
    /// State-Cleanup.
    /// </summary>
    public virtual void DoExitState() { CurrentState?.DoExitState(); ResetValues(); }

    /// <summary>
    /// This method is called once every frame while this state is active.
    /// Consider this the "Update" method of this state.
    /// </summary>
    public virtual void DoUpdateState() { CheckTransitions(); HandleTimer(); }

    /// <summary>
    /// This method is called once every physics frame while this state is active.
    /// Consider this the "FixedUpdate" method of this state.
    /// </summary>
    public virtual void DoFixedUpdateState() { }
    

    /// <summary>
    /// This method is called during ExitLogic().
    /// Use this method to reset or null out values during state cleanup.
    /// </summary>
    public virtual void ResetValues() { StateUptime = 0f; IsComplete = false; }

    /// <summary>
    /// This method contains checks for all transitions from the current state.
    /// To be called in the UpdateState() or FixedUpdateState() methods.
    /// </summary>
    public virtual void CheckTransitions() { }


    protected void HandleTimer()
    {
        StateUptime += Time.deltaTime;
    }

    /// <summary>
    /// Calls DoUpdate for every state down the branch
    /// </summary>
    public void DoUpdateBranch()
    {
        CurrentState?.DoUpdateBranch();
        DoUpdateState();
    }

    /// <summary>
    /// Calls DoFixedUpdate for every state down the branch
    /// </summary>
    public void DoFixedUpdateBranch()
    {
        CurrentState?.DoFixedUpdateBranch();
        DoFixedUpdateState();
    }

}