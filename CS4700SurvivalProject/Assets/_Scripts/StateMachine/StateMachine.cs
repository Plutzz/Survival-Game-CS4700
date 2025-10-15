using System.Collections;
using System.Collections.Generic;
using System;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Allows for the creation of a state machine to define more advanced player, npc, and enemy behavior
/// </summary>
public class StateMachine<TContext>
{
    public TContext Context { get; private set; }
    public State<TContext> CurrentState { get; private set; }
    public State<TContext> PreviousState { get; private set; }

    public StateMachine(TContext context)
    {
        Context = context;
    }
    
    // This event is called when a state is changed and passes the previous state (from state) and new state (to state)
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State<TContext> PreviousState;
        public State<TContext> NextState;
    } 
    
    
    
    /// <summary>
    /// Calls Update on the current state in this state machine
    /// </summary>
    public void Update()
    {
        CurrentState.UpdateState();
    }


    /// <summary>
    /// Calls Update on the current state in this state machine
    /// </summary>
    public void FixedUpdate()
    {
        CurrentState.FixedUpdateState();
    }

    /// <summary>
    /// Sets the state machine with a specified state
    /// </summary>
    /// <param name="_newState"></param>
    /// <param name="_forceReset"</param>
    public void SetState(State<TContext> _newState, bool _forceReset = false)
    {
        if (CurrentState != _newState || _forceReset)
        {
            //Debug.Log("Changing State to " + _newState);
            CurrentState?.ExitState();
            PreviousState = CurrentState;
            CurrentState = _newState;
            CurrentState.EnterState();
            OnStateChanged?.Invoke(this,
                new OnStateChangedEventArgs { PreviousState = PreviousState, NextState = CurrentState });
        }

    }

    [ClientRpc]
    public void SetStateClientRPC(State<TContext> _newState, bool _forceReset = false)
    {
        SetState(_newState, _forceReset);
    }
    [ServerRpc]
    public void SetStateServerRPC(State<TContext> _newState, bool _forceReset = false)
    {
        SetState(_newState, _forceReset);
    }
    


    public List<State<TContext>> GetActiveStateBranch(List<State<TContext>> _list = null)
    {
        if (_list == null)
            _list = new List<State<TContext>>();

        if (CurrentState == null)
            return _list;

        else
        {
            _list.Add(CurrentState);
            return CurrentState.StateMachine.GetActiveStateBranch(_list);
        }

    }

}