using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : State
{
    [SerializeField] private AnimationClip up, side, down;
    [SerializeField] private Player player;
    [SerializeField] private float attackTime = 0.5f;


    public override void DoEnterState()
    {
        if (Mathf.Abs(player.lastMoveDir.x) > Mathf.Abs(player.lastMoveDir.y))
        {
            animator.Play(side.name);
            if (player.lastMoveDir.x > 0)
                player.pivot.localScale = new Vector3(1f, 1f, 1f);
            else
                player.pivot.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            if (player.lastMoveDir.y > 0)
                animator.Play(up.name);
            else
                animator.Play(down.name);
        }
        base.DoEnterState();
    }
    public override void DoUpdateState()
    {
        Vector2 moveInput = player.playerInput.moveVector.normalized;
        rb.velocity = moveInput * player.stats.moveSpeed;
        
        if (stateUptime > attackTime)
        {
            isComplete = true;
        }
        
        base.DoUpdateState();
    }
}
