using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEngine.KeyCode;

public class InputManager : Singleton<InputManager>
{
    [Header("References")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private EventSystem eventSystem;

    private InputActionMap playerActionMap, uiActionMap;
    
    private CursorLockMode desiredCursorLockState;

    #region Control Scheme
    public enum ControlScheme
    {
        KeyboardMouse,
        Gamepad
    }
    
    [field: Header("Control Scheme")]
    [field: SerializeField, ReadOnly] public ControlScheme CurrentControlScheme { get; private set; }
    
    public event Action<ControlScheme> OnControlSchemeChanged = delegate { };
    
    #endregion
    
    #region Controls
    // Move
    public Vector2 MoveVector { get; private set; }
    public float TimeOfLastMoveInput { get; private set; }
    
    // Interact
    
    public bool InteractPressedDownThisFrame { get; private set; }
    public bool InteractHeld { get; private set; }
    public bool InteractReleasedThisFrame { get; private set; }
    
    public bool AttackPressedDownThisFrame { get; private set; }
    
    public bool AttackHeld { get; private set; }
    public bool AttackReleasedThisFrame { get; private set; }
    public bool ResetInput { get; private set;}
    #endregion

    protected override void Awake()
    {
        base.Awake();
        playerActionMap = inputActions.FindActionMap("Player");
        uiActionMap = inputActions.FindActionMap("UI");
        SetCallbacks();
    }
    
    
    void Update()
    {
        MoveVector = playerActionMap.FindAction("Move").ReadValue<Vector2>();
        
        if (MoveVector.magnitude > 0)
        {
            TimeOfLastMoveInput = Time.time;
        }
        
        InteractPressedDownThisFrame = playerActionMap.FindAction("Interact").WasPerformedThisFrame();
        InteractHeld = playerActionMap.FindAction("Interact").ReadValue<float>() > 0;
        InteractReleasedThisFrame = playerActionMap.FindAction("Interact").WasReleasedThisFrame();
        
        AttackPressedDownThisFrame = playerActionMap.FindAction("Attack").WasPerformedThisFrame();
        AttackHeld = playerActionMap.FindAction("Attack").ReadValue<float>() > 0;
        AttackReleasedThisFrame = playerActionMap.FindAction("Attack").WasReleasedThisFrame();
        
        // DEBUG INPUTS
        ResetInput = Input.GetKeyDown(R);
    }


    private void SetCallbacks()
    {
        playerActionMap.FindAction("Pause").performed += OnPause;
    }
    
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void EnablePlayerInput()
    {
        inputActions.Enable();
        playerActionMap.Enable();
        uiActionMap.Disable();
        eventSystem.sendNavigationEvents = false;
        
        LockCursor(true);
    }

    public void DisableAllInput()
    {
        inputActions.Disable();
        playerActionMap.Disable();
        uiActionMap.Disable();
    }

    public void EnableUIInput()
    {
        inputActions.Enable();
        playerActionMap.Disable();
        uiActionMap.Enable();
        eventSystem.sendNavigationEvents = true;
        
        LockCursor(false);
        
    }


    private void OnPause(InputAction.CallbackContext context)
    {
        if(context.performed)
            GameManager.Instance.ChangeGameState(GameState.Paused);
    }
    
    //
    // private void PlayerInput_OnControlsChanged(PlayerInput input)
    // {
    //     ControlScheme newScheme = input.currentControlScheme == "Gamepad" ? ControlScheme.Gamepad : ControlScheme.KeyboardMouse;
    //
    //     if (newScheme != CurrentControlScheme)
    //     {
    //         CurrentControlScheme = newScheme;
    //         OnControlSchemeChanged.Invoke(CurrentControlScheme);
    //
    //         ApplyCursorLockState();
    //     }
    // }
    
    /// <summary>
    /// Locks or unlocks the cursor based on the user's preference.
    /// Keeps track of the desired cursor lock state, which is used when switching between control schemes.
    /// Gamepad always locks the cursor.
    /// </summary>
    /// <param name="locked"></param>
    public void LockCursor(bool locked)
    {
        // Always remember the user's preference
        desiredCursorLockState = locked ? CursorLockMode.Locked : CursorLockMode.None;

        ApplyCursorLockState();
    }
    
    private void ApplyCursorLockState()
    {
        if (CurrentControlScheme == ControlScheme.Gamepad)
        {
            // Gamepad always locks
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }

        Cursor.lockState = desiredCursorLockState;
        Cursor.visible = desiredCursorLockState != CursorLockMode.Locked;
    }
}
