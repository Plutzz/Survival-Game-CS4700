using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : State
{
    [SerializeField] private AnimationClip up, right, left, down;
    [SerializeField] private Player player;

    public override void DoEnterState()
    {
        rb.velocity = new Vector2(0, 0);
        base.DoEnterState();
    }
    public override void DoUpdateState()
    {
        if (Mathf.Abs(player.lastMoveDir.x) > Mathf.Abs(player.lastMoveDir.y))
        {
            if (player.lastMoveDir.x > 0)
                animator.Play(right.name);
            else
                animator.Play(left.name);
        }
        else
        {
            if (player.lastMoveDir.y > 0)
                animator.Play(up.name);
            else
                animator.Play(down.name);
        }
        base.DoUpdateState();
    }
}
