using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : State
{
    [SerializeField] private AnimationClip up, right, left, down;
    [SerializeField] private Player player;

    public override void DoEnterState()
    {
        Rb.velocity = new Vector2(0, 0);
        base.DoEnterState();
    }
    public override void DoUpdateState()
    {
        if (Mathf.Abs(player.lookDir.Value.x) > Mathf.Abs(player.lookDir.Value.y))
        {
            if (player.lookDir.Value.x > 0)
                Animator.Play(right.name);
            else
                Animator.Play(left.name);
        }
        else
        {
            if (player.lookDir.Value.y > 0)
                Animator.Play(up.name);
            else
                Animator.Play(down.name);
        }
        base.DoUpdateState();
    }
}
