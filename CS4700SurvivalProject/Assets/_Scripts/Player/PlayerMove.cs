using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : State
{
    [SerializeField] private Player player;
    [SerializeField] private AnimationClip up, right, left, down;
    [SerializeField] private float itemBobSpeed;
    [SerializeField] private float itemBobAmount;
    [SerializeField] private Transform itemHold;
    private PlayerStats stats => player.stats;
    private PlayerInput input => player.playerInput;
    public override void DoUpdateState()
    {
        Vector2 moveInput = input.moveVector.normalized;
        Rb.velocity = moveInput * stats.moveSpeed;
        
        // Item Bob
        float step = 1f / 16f; // 0.0625
        float newY = Mathf.Sin(Time.time * itemBobSpeed) * itemBobAmount;
        newY = Mathf.Round(newY / step) * step;
        itemHold.localPosition = new Vector3(0, newY, 0);
        
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

    public override void DoExitState()
    {
        Debug.Log("Reset Item Hold");
        itemHold.localPosition = new Vector3(0, 0, 0);
        base.DoExitState();
    }
}
