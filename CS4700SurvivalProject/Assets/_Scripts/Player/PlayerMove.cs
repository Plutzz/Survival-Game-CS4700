using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : State
{
    [SerializeField] private Player player;
    [SerializeField] private AnimationClip up, down, left, right;
    private PlayerStats stats => player.stats;
    private PlayerInput input => player.playerInput;
    public override void DoUpdateState()
    {
        Vector2 moveInput = input.moveVector.normalized;
        rb.velocity = moveInput * stats.moveSpeed;
        
        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            if (moveInput.x > 0)
                animator.Play(right.name);
            else
                animator.Play(left.name);
        }
        else
        {
            if (moveInput.y > 0)
                animator.Play(up.name);
            else
                animator.Play(down.name);
        }
        base.DoUpdateState();
    }

    public override void DoExitState()
    {
        
    }
}
