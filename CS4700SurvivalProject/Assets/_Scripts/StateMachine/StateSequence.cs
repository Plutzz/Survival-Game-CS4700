using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This state plays all states in the list in order one after another
public class StateSequence : State
{
    [SerializeField] private List<State> states;
    private int currentStateIndex;

    public override void DoEnterState()
    {
        base.DoEnterState();
        currentStateIndex = 0;
        StateMachine.SetState(states[currentStateIndex], true);
    }
    public override void CheckTransitions()
    {
        base.CheckTransitions();
        //Debug.Log(gameObject.name + " Ind: " + currentStateIndex + "Count: " + states.Count);

        if (!CurrentState.IsComplete) return;
        // If we are not on the last state go to the next state
        if (currentStateIndex != states.Count - 1)
        {
            currentStateIndex++;
            //Debug.Log(gameObject.name + " Changing State to " + states[currentStateIndex]);
            StateMachine.SetState(states[currentStateIndex], true);
        }
        // If we are on the last state, mark this state as true
        else
        {
            IsComplete = true;
        }
    }
}
