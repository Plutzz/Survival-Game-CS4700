using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEngine.KeyCode;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerInputAction;
    public Vector2 moveVector { get; private set; }
    public float timeOfLastMoveInput { get; private set; }
    public bool attackPressedDownThisFrame { get; private set; }
    public bool attackHeld { get; private set; }
    public bool attackReleasedThisFrame { get; private set; }
    public bool sprintHeld { get; private set; }
    public bool sprintPressedThisFrame { get; private set; }
    public bool ResetInput { get; private set;}

    private void OnEnable()
    {
        playerInputAction.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Disable();
    }


    void Update()
    {
        InputActionMap playerActionMap = playerInputAction.actionMaps[0];
        moveVector = playerActionMap.FindAction("Move").ReadValue<Vector2>();
        
        
        if (moveVector.magnitude > 0)
        {
            timeOfLastMoveInput = Time.time;
        }
        
        // // Attack
        // attackPressedDownThisFrame = playerActionMap.FindAction("Attack").WasPerformedThisFrame();
        // attackHeld = playerActionMap.FindAction("Attack").ReadValue<float>() > 0;
        // attackReleasedThisFrame = playerActionMap.FindAction("Attack").WasReleasedThisFrame();
        //
        // // Sprint
        // sprintHeld = playerActionMap.FindAction("Sprint").ReadValue<float>() > 0;
        // sprintPressedThisFrame = playerActionMap.FindAction("Attack").WasPerformedThisFrame();
        
        // DEBUG INPUTS
        ResetInput = Input.GetKeyDown(R);
    }
}
