using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
        if (Mathf.Abs(player.lookDir.Value.x) > Mathf.Abs(player.lookDir.Value.y))
        {

            if (player.lookDir.Value.x > 0)
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
            if (player.lookDir.Value.y > 0)
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
        Rb.velocity = moveInput * player.stats.moveSpeed;
        
        if (StateUptime > attackTime)
        {
            IsComplete = true;
        }
        
        base.DoUpdateState();
    }
}
