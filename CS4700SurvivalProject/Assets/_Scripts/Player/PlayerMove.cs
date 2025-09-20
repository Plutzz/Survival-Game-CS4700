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
        rb.velocity = moveInput * stats.moveSpeed;
        
        // Item Bob
        float newY = Mathf.Sin(Time.time * itemBobSpeed) * itemBobAmount;
        itemHold.localPosition = new Vector3(0, newY, 0);
        
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

    public override void DoExitState()
    {
        Debug.Log("Reset Item Hold");
        itemHold.localPosition = new Vector3(0, 0, 0);
        base.DoExitState();
    }
}
