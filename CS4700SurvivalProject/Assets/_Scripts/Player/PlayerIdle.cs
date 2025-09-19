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
}
