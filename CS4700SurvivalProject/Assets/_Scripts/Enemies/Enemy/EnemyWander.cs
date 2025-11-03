using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyWander : State<Enemy>
{
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private float speed = 1, wanderRadius = 2, maxTime = 5;
    private Vector3 _target;
    public override void EnterState()
    {
        base.EnterState();
        if (animationClip != null)
        {
            Context.animator?.Play(animationClip.name);
        }
        _target = Context.transform.position + new Vector3(Random.Range(-wanderRadius, wanderRadius), Random.Range(-wanderRadius, wanderRadius));
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
        
        Vector2 direction = (_target - Context.transform.position).normalized;
        Context.rb.velocity = direction * speed;
        
        Context.animator.transform.localScale = new Vector3(Context.rb.velocity.x < 0 ? 1 : -1, Context.animator.transform.localScale.y, Context.animator.transform.localScale.z);
        
        IsComplete = StateUptime > maxTime || Vector3.Distance(_target, Context.transform.position) < 0.2f;
    }
}