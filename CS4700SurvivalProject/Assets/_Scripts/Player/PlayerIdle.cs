using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : State
{
    [SerializeField] private AnimationClip up, down, left, right;
    [SerializeField] private Player player;

    public override void DoEnterState()
    {
        rb.velocity = new Vector2(0, 0);
        base.DoEnterState();
    }
    public override void DoUpdateState()
    {
        if (Mathf.Abs(player.moveInput.x) > Mathf.Abs(player.moveInput.y))
        {
            if (player.moveInput.x > 0)
                animator.Play(right.name);
            else
                animator.Play(left.name);
        }
        else
        {
            if (player.moveInput.y > 0)
                animator.Play(up.name);
            else
                animator.Play(down.name);
        }
        base.DoUpdateState();
    }
}
