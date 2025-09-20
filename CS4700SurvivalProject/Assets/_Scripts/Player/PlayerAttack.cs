using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAttack : State
{
    [SerializeField] private AnimationClip up, right, left, down;
    [SerializeField] private Player player;
    [SerializeField] private float attackTime = 0.5f;
    [SerializeField] private DamageBox weaponHitbox;

    public override void DoEnterState()
    {
        weaponHitbox.gameObject.SetActive(true);
        weaponHitbox.ClearHasBeenDamaged();
        if (Mathf.Abs(player.lookDir.x) > Mathf.Abs(player.lookDir.y))
        {

            if (player.lookDir.x > 0)
            {
                weaponHitbox.transform.localPosition = new Vector3(-Mathf.Abs(weaponHitbox.transform.localPosition.x),
                    weaponHitbox.transform.localPosition.y, weaponHitbox.transform.localPosition.z);
                animator.Play(right.name);
            }

            else
            {
                weaponHitbox.transform.localPosition = new Vector3(Mathf.Abs(weaponHitbox.transform.localPosition.x),
                    weaponHitbox.transform.localPosition.y, weaponHitbox.transform.localPosition.z);
                animator.Play(left.name);
            }
                
        }
        else
        {
            if (player.lookDir.y > 0)
            {
                weaponHitbox.transform.localPosition = new Vector3(Mathf.Abs(weaponHitbox.transform.localPosition.x),
                    weaponHitbox.transform.localPosition.y, weaponHitbox.transform.localPosition.z);
                animator.Play(up.name);
            }

            else
            {
                weaponHitbox.transform.localPosition = new Vector3(-Mathf.Abs(weaponHitbox.transform.localPosition.x),
                    weaponHitbox.transform.localPosition.y, weaponHitbox.transform.localPosition.z);
                animator.Play(down.name);
            }
                
        }
        base.DoEnterState();
    }

    public override void DoExitState()
    {
        weaponHitbox.gameObject.SetActive(false);
        base.DoExitState();
    }
    public override void DoUpdateState()
    {
        Vector2 moveInput = player.playerInput.moveVector.normalized;
        rb.velocity = moveInput * player.stats.moveSpeed;
        
        if (stateUptime > attackTime)
        {
            isComplete = true;
        }
        
        base.DoUpdateState();
    }
}
