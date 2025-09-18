using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : State
{
    [SerializeField] private Player player;
    [SerializeField] private AnimationClip up, side, down;
    private PlayerStats stats => player.stats;
    private PlayerInput input => player.playerInput;
    public override void DoUpdateState()
    {
        Vector2 moveInput = input.moveVector.normalized;
        rb.velocity = moveInput * stats.moveSpeed;
        
        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            animator.Play(side.name);
            if (moveInput.x > 0)
                player.pivot.localScale = new Vector3(1f, 1f, 1f);
            else
                player.pivot.localScale = new Vector3(-1f, 1f, 1f);
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
