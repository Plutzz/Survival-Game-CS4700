using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Serialization;

public class Player : StateMachineCore
{
    [field: HorizontalLine(color: EColor.Gray)]
    [field: Header("States")]
    [field: SerializeField] public PlayerIdle idle { get; private set; }
    [field: SerializeField] public PlayerMove move { get; private set; }

    [Header("Components")] 
    [SerializeField] public PlayerInput playerInput;

    [SerializeField] public PlayerStats stats;
    [SerializeField] private float speed;
    
    [FormerlySerializedAs("moveinput")] [FormerlySerializedAs("lastMoveDir")] public Vector2 moveInput;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupInstances();
        stateMachine.SetState(idle);
    }

    // Update is called once per frame
    void Update()
    {
        HandleTransitions();
        if (playerInput.moveVector != Vector2.zero)
        {
            moveInput = playerInput.moveVector;
        }
        stateMachine.currentState.DoUpdateBranch();
    }

    void FixedUpdate()
    {
        stateMachine.currentState.DoFixedUpdateBranch();
    }

    private void HandleTransitions()
    {
        if (stateMachine.currentState == idle && playerInput.moveVector != Vector2.zero)
        {
            stateMachine.SetState(move);
            return;
        }

        if (stateMachine.currentState == move && playerInput.moveVector == Vector2.zero)
        {
            stateMachine.SetState(idle);
            return;
        }
    }
}
