using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : State
{
    [SerializeField] private Player player;
    [SerializeField] private AnimationClip up, right, left, down;
    private PlayerStats stats => player.stats;
    private PlayerInput input => player.playerInput;
    public override void DoUpdateState()
    {
        Vector2 moveInput = input.moveVector.normalized;
        rb.velocity = moveInput * stats.moveSpeed;
        
        if (Mathf.Abs(player.lookDir.x) > Mathf.Abs(player.lookDir.y))
        {
            
            if (player.lookDir.x > 0)
                animator.Play(right.name);
            else
                animator.Play(left.name);
        }
        else
        {
            if (player.lookDir.y > 0)
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
