using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : State<Player
{
    [SerializeField] private AnimationClip up, right, left, down;
    [SerializeField] private float itemBobSpeed;
    [SerializeField] private float itemBobAmount;
    [SerializeField] private Transform itemHold;
    private PlayerStats stats => Context.stats;
    private PlayerInput input => Context.playerInput;
    public override void UpdateState()
    {
        Vector2 moveInput = input.moveVector.normalized;
        Rb.velocity = moveInput * stats.moveSpeed;
        
        // Item Bob
        float step = 1f / 16f; // 0.0625
        float newY = Mathf.Sin(Time.time * itemBobSpeed) * itemBobAmount;
        newY = Mathf.Round(newY / step) * step;
        itemHold.localPosition = new Vector3(0, newY, 0);
        
        if (Mathf.Abs(Context.lookDir.Value.x) > Mathf.Abs(Context.lookDir.Value.y))
        {
            
            if (Context.lookDir.Value.x > 0)
                Animator.Play(right.name);
            else
                Animator.Play(left.name);
        }
        else
        {
            if (Context.lookDir.Value.y > 0)
                Animator.Play(up.name);
            else
                Animator.Play(down.name);
        }
        base.UpdateState();
    }

    public override void ExitState()
    {
        Debug.Log("Reset Item Hold");
        itemHold.localPosition = new Vector3(0, 0, 0);
        base.ExitState();
    }
}
