using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerMove : State<Player>
{
    [SerializeField] public AnimationClip Up, Right, Left, Down;
    [SerializeField] public float ItemBobSpeed;
    [SerializeField] public float ItemBobAmount;
    [SerializeField] public Transform ItemHold;
    private PlayerStats _stats => Context.Stats;
    private PlayerInput _input => Context.PlayerInput;
    public override void UpdateState()
    {
        Vector2 moveInput = _input.moveVector.normalized;
        Context.Rb.velocity = moveInput * _stats.moveSpeed;
        
        // Item Bob
        float step = 1f / 16f; // 0.0625
        float newY = Mathf.Sin(Time.time * ItemBobSpeed) * ItemBobAmount;
        newY = Mathf.Round(newY / step) * step;
        ItemHold.localPosition = new Vector3(0, newY, 0);
        
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

    public override void ExitState()
    {
        Debug.Log("Reset Item Hold");
        ItemHold.localPosition = new Vector3(0, 0, 0);
        base.ExitState();
    }
}
