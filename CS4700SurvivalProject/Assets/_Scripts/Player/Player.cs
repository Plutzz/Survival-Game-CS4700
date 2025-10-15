using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.Netcode;
using UnityEngine.Serialization;

public class Player : NetworkBehaviour
{
    public StateMachine<Player> StateMachine { get; private set; }
    [field: HorizontalLine(color: EColor.Gray)]
    [field: Header("States")]
    
    
    
    [Header("Components")] 
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] public Transform pivot;
    [SerializeField] public PlayerStats stats;
    [SerializeField] private float speed;

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
        SetupInstances();
        SetState(allStates["Idle"]);
        SetStateServerRpc("Idle", false);
        
        if(!bypassNetwork && !IsOwner) return; 
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(!bypassNetwork && !IsOwner) return;
        
        HandleTransitions();
        
        if (PauseMenuManager.Instance != null && !PauseMenuManager.Instance.isGamePaused)
        {
            lookDir.Value = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        }
        
        CameraManager.Instance?.SetPlayerFollow(transform);
        
        CurrentState.DoUpdateBranch();
    }

    protected override void FixedUpdate()
    {
        if(!bypassNetwork && !IsOwner) return;
        
        CurrentState.DoFixedUpdateBranch();
    }

    private void HandleTransitions()
    {
        
        if ((CurrentState == allStates["Move"] || CurrentState == allStates["Idle"]) &&
            playerInput.attackPressedDownThisFrame)
        {
            SetState(allStates["Attack"]);
            SetStateServerRpc("Attack", false);
            return;
        }
        
        if ((CurrentState == allStates["Idle"] || CurrentState.IsComplete) && playerInput.moveVector != Vector2.zero)
        {
            SetState(allStates["Move"]);
            SetStateServerRpc("Move", false);
            return;
        }
        
        if ((CurrentState == allStates["Move"] || CurrentState.IsComplete) && playerInput.moveVector == Vector2.zero)
        {
            SetState(allStates["Idle"]);
            SetStateServerRpc("Idle", false);
            return;
        }
    }

    private void ChangeState(string newState, bool forceReset)
    {
        SetState(allStates[newState], forceReset);
        
        if(!bypassNetwork && !IsOwner) return;
        
        SetStateServerRpc(newState, forceReset);
    }
}
