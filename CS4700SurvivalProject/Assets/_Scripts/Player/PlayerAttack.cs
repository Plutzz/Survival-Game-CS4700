using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : State
{
    [SerializeField] private AnimationClip up, right, left, down;
    [SerializeField] private Player player;
    [SerializeField] private float attackTime = 0.5f;
    [SerializeField] private GameObject weaponSprite, weaponPivot;
    [SerializeField] private WeaponAttack weapon;

    public override void DoEnterState()
    {
        weaponSprite.SetActive(true);
        if (Mathf.Abs(player.lastMoveDir.x) > Mathf.Abs(player.lastMoveDir.y))
        {

            if (player.lastMoveDir.x > 0)
            {
                weaponSprite.transform.localPosition = new Vector3(-Mathf.Abs(weaponSprite.transform.localPosition.x),
                    weaponSprite.transform.localPosition.y, weaponSprite.transform.localPosition.z);
                animator.Play(right.name);
                weaponPivot.transform.localEulerAngles = new Vector3(0, 0, 90);
            }

            else
            {
                weaponSprite.transform.localPosition = new Vector3(Mathf.Abs(weaponSprite.transform.localPosition.x),
                    weaponSprite.transform.localPosition.y, weaponSprite.transform.localPosition.z);
                animator.Play(left.name);
                weaponPivot.transform.localEulerAngles = new Vector3(0, 0, -90);
            }
                
        }
        else
        {
            if (player.lastMoveDir.y > 0)
            {
                weaponSprite.transform.localPosition = new Vector3(Mathf.Abs(weaponSprite.transform.localPosition.x),
                    weaponSprite.transform.localPosition.y, weaponSprite.transform.localPosition.z);
                animator.Play(up.name);
                weaponPivot.transform.localEulerAngles = new Vector3(0, 0, 180);
            }

            else
            {
                weaponSprite.transform.localPosition = new Vector3(-Mathf.Abs(weaponSprite.transform.localPosition.x),
                    weaponSprite.transform.localPosition.y, weaponSprite.transform.localPosition.z);
                animator.Play(down.name);
                weaponPivot.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
                
        }
        weapon.Attack();
        base.DoEnterState();
    }

    public override void DoExitState()
    {
        weaponSprite.SetActive(false);
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
