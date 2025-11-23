using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyIdle : State<Enemy>
{
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private float maxTime = 2;
    private Vector3 _target;
    public override void EnterState()
    {
        base.EnterState();
        if (animationClip != null)
        {
            Context.animator?.Play(animationClip.name);
        }
        
        Context.rb.velocity = Vector3.zero;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        
        if (!Context.IsServer) return;
        
        IsComplete = StateUptime > maxTime;
    }
}