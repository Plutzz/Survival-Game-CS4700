using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : State<Player>
{
    [SerializeField] private AnimationClip up, right, left, down;

    public override void EnterState()
    {
        Rb.velocity = new Vector2(0, 0);
        base.EnterState();
    }
    public override void UpdateState()
    {
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
}
