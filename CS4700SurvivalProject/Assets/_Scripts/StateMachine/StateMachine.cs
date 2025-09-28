using System.Collections;
using System.Collections.Generic;
using System;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Allows for the creation of a state machine to define more advanced player, npc, and enemy behavior
/// </summary>
public class StateMachine
{
    public State currentState { get; private set; }
    public State previousState { get; private set; }

    // This event is called when a state is changed and passes the previous state (from state) and new state (to state)
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State previousState;
        public State nextState;
    }


    /// <summary>
    /// Sets the state machine with a specified state
    /// </summary>
    /// <param name="_newState"></param>
    /// <param name="_forceReset"</param>
    [ServerRpc]
    public void SetState(State _newState, bool _forceReset = false)
    {
        if (currentState != _newState || _forceReset)
        {
            //Debug.Log("Changing State to " + _newState);
            currentState?.DoExitState();
            currentState?.gameObject.SetActive(false);
            previousState = currentState;
            currentState = _newState;
            currentState.gameObject.SetActive(true);
            currentState.DoEnterState();
            OnStateChanged?.Invoke(this,
                new OnStateChangedEventArgs { previousState = previousState, nextState = currentState });
        }

    }

    [ClientRpc]
    public void SetStateClientRPC(State _newState, bool _forceReset = false)
    {
        SetState(_newState, _forceReset);
    }
    [ServerRpc]
    public void SetStateServerRPC(State _newState, bool _forceReset = false)
    {
        SetState(_newState, _forceReset);
    }
    


public List<State> GetActiveStateBranch(List<State> _list = null)
    {
        if (_list == null)
            _list = new List<State>();

        if (currentState == null)
            return _list;

        else
        {
            _list.Add(currentState);
            return currentState.stateMachine.GetActiveStateBranch(_list);
        }

    }

}