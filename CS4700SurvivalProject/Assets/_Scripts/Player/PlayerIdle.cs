using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerIdle : State<Player>
{
    [SerializeField] public AnimationClip Up, Right, Left, Down;

    public override void EnterState()
    {
        Context.Rb.velocity = new Vector2(0, 0);
        base.EnterState();
    }
    public override void UpdateState()
    {
        if (Mathf.Abs(Context.lookDir.Value.x) > Mathf.Abs(Context.lookDir.Value.y))
        {
            if (Context.lookDir.Value.x > 0)
                Context.Animator.Play(Right.name);
            else
                Context.Animator.Play(Left.name);
        }
        else
        {
            if (Context.lookDir.Value.y > 0)
                Context.Animator.Play(Up.name);
            else
                Context.Animator.Play(Down.name);
        }
        base.UpdateState();
    }
}
