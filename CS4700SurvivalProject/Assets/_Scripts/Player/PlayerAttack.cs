using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAttack : State<Player>
{
    [SerializeField] private AnimationClip up, right, left, down;
    [SerializeField] private float attackTime = 0.5f;
    [SerializeField] private DamageBox weaponHitbox;

    public override void EnterState()
    {
        base.EnterState();
        weaponHitbox.gameObject.SetActive(true);
        weaponHitbox.ClearHasBeenDamaged();
        if (Mathf.Abs(Context.lookDir.Value.x) > Mathf.Abs(Context.lookDir.Value.y))
        {

            if (Context.lookDir.Value.x > 0)
            {
                weaponHitbox.transform.localPosition = new Vector3(-Mathf.Abs(weaponHitbox.transform.localPosition.x),
                    weaponHitbox.transform.localPosition.y, weaponHitbox.transform.localPosition.z);
                Animator.Play(right.name);
            }

            else
            {
                weaponHitbox.transform.localPosition = new Vector3(Mathf.Abs(weaponHitbox.transform.localPosition.x),
                    weaponHitbox.transform.localPosition.y, weaponHitbox.transform.localPosition.z);
                Animator.Play(left.name);
            }
                
        }
        else
        {
            if (Context.lookDir.Value.y > 0)
            {
                weaponHitbox.transform.localPosition = new Vector3(Mathf.Abs(weaponHitbox.transform.localPosition.x),
                    weaponHitbox.transform.localPosition.y, weaponHitbox.transform.localPosition.z);
                Animator.Play(up.name);
            }

            else
            {
                weaponHitbox.transform.localPosition = new Vector3(-Mathf.Abs(weaponHitbox.transform.localPosition.x),
                    weaponHitbox.transform.localPosition.y, weaponHitbox.transform.localPosition.z);
                Animator.Play(down.name);
            }
                
        }
        
    }

    public override void ExitState()
    {
        base.ExitState();
        weaponHitbox.gameObject.SetActive(false);
        
    }
    public override void UpdateState()
    {
        base.UpdateState();
        
        Vector2 moveInput = Context.playerInput.moveVector.normalized;
        Rb.velocity = moveInput * Context.stats.moveSpeed;
        
        if (StateUptime > attackTime)
        {
            IsComplete = true;
        }
        
        
    }
}
