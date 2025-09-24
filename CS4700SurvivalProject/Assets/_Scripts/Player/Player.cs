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
    [field: SerializeField] public PlayerAttack attack { get; private set; }

    [Header("Components")] 
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] public Transform pivot;
    [SerializeField] public PlayerStats stats;
    [SerializeField] private float speed;
    
    public Vector2 lookDir;
    
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
        if (!PauseMenuManager.Instance.isGamePaused)
        {
            lookDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        }
        stateMachine.currentState.DoUpdateBranch();
    }

    void FixedUpdate()
    {
        stateMachine.currentState.DoFixedUpdateBranch();
    }

    private void HandleTransitions()
    {
        
        if ((stateMachine.currentState == move || stateMachine.currentState == idle) &&
            playerInput.attackPressedDownThisFrame)
        {
            stateMachine.SetState(attack);
            return;
        }
        
        if ((stateMachine.currentState == idle || stateMachine.currentState.isComplete) && playerInput.moveVector != Vector2.zero)
        {
            stateMachine.SetState(move);
            return;
        }

        if ((stateMachine.currentState == move || stateMachine.currentState.isComplete) && playerInput.moveVector == Vector2.zero)
        {
            stateMachine.SetState(idle);
            return;
        }
    }
}
