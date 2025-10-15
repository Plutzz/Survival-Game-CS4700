using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.Netcode;
using UnityEngine.Serialization;

public class Player : NetworkBehaviour
{
    [field:Header("Components")] 
    [field:SerializeField] public PlayerInput PlayerInput { get; private set; }
    [field:SerializeField] public Transform Pivot { get; private set; }
    [field:SerializeField] public PlayerStats Stats { get; private set; }
    [field:SerializeField] public Rigidbody2D Rb { get; private set; }
    [field:SerializeField] public Animator Animator { get; private set; }
    [field:SerializeField] public float Speed { get; private set; }
    
    [field: HorizontalLine(color: EColor.Gray)]
    [field: Header("State Machine")]
    public StateMachine<Player> StateMachine { get; private set; }
    [field: SerializeField] public PlayerMove MoveState { get; private set; } = new();
    [field: SerializeField] public PlayerIdle IdleState { get; private set; } = new();
    [field: SerializeField] public PlayerAttack AttackState  { get; private set; } = new();

    [Header("Debug")]
    [SerializeField] private bool bypassNetwork;
    
    public NetworkVariable<Vector2> lookDir = new NetworkVariable<Vector2>(
        Vector2.zero, 
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    
    // Start is called before the first frame update
    void Start()
    {
        SetupStateMachine();
        StateMachine.SetState(IdleState);
        // SetStateServerRpc("Idle", false);
        
        if(!bypassNetwork && !IsOwner) return; 
    }

    // Update is called once per frame
    private void Update()
    {
        if(!bypassNetwork && !IsOwner) return;
        
        HandleTransitions();
        
        if (PauseMenuManager.Instance != null && !PauseMenuManager.Instance.isGamePaused)
        {
            lookDir.Value = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        }
        
        CameraManager.Instance?.SetPlayerFollow(transform);
        
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        if(!bypassNetwork && !IsOwner) return;
        
        StateMachine.FixedUpdate();
    }

    private void HandleTransitions()
    {
        
        if ((StateMachine.CurrentState == MoveState || StateMachine.CurrentState == IdleState) &&
            PlayerInput.attackPressedDownThisFrame)
        {
            StateMachine.SetState(AttackState);
            // SetStateServerRpc("Attack", false);
            return;
        }
        
        if ((StateMachine.CurrentState == IdleState || StateMachine.CurrentState.IsComplete) && PlayerInput.moveVector != Vector2.zero)
        {
            StateMachine.SetState(MoveState);
            // SetStateServerRpc("Move", false);
            return;
        }
        
        if ((StateMachine.CurrentState == MoveState || StateMachine.CurrentState.IsComplete) && PlayerInput.moveVector == Vector2.zero)
        {
            StateMachine.SetState(IdleState);
            // SetStateServerRpc("Idle", false);
            return;
        }
    }

    private void SetupStateMachine()
    {
        StateMachine = new StateMachine<Player>(this);
        MoveState.Init(this);
        IdleState.Init(this);
        AttackState.Init(this);
        
    }
    // private void ChangeState(string newState, bool forceReset)
    // {
    //     StateMachine.SetState(allStates[newState], forceReset);
    //     
    //     if(!bypassNetwork && !IsOwner) return;
    //     
    //     SetStateServerRpc(newState, forceReset);
    // }
}
