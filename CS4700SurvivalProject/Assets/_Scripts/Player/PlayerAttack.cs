using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerAttack : State<Player>
{
    [SerializeField] public AnimationClip Up, Right, Left, Down;
    [SerializeField] public float AttackTime = 0.5f;
    [SerializeField] public DamageBox WeaponHitbox;

    public override void EnterState()
    {
        base.EnterState();
        WeaponHitbox.gameObject.SetActive(true);
        WeaponHitbox.ClearHasBeenDamaged();
        if (Mathf.Abs(Context.lookDir.Value.x) > Mathf.Abs(Context.lookDir.Value.y))
        {

            if (Context.lookDir.Value.x > 0)
            {
                WeaponHitbox.transform.localPosition = new Vector3(-Mathf.Abs(WeaponHitbox.transform.localPosition.x),
                    WeaponHitbox.transform.localPosition.y, WeaponHitbox.transform.localPosition.z);
                Context.Animator.Play(Right.name);
            }

            else
            {
                WeaponHitbox.transform.localPosition = new Vector3(Mathf.Abs(WeaponHitbox.transform.localPosition.x),
                    WeaponHitbox.transform.localPosition.y, WeaponHitbox.transform.localPosition.z);
                Context.Animator.Play(Left.name);
            }
                
        }
        else
        {
            if (Context.lookDir.Value.y > 0)
            {
                WeaponHitbox.transform.localPosition = new Vector3(Mathf.Abs(WeaponHitbox.transform.localPosition.x),
                    WeaponHitbox.transform.localPosition.y, WeaponHitbox.transform.localPosition.z);
                Context.Animator.Play(Up.name);
            }

            else
            {
                WeaponHitbox.transform.localPosition = new Vector3(-Mathf.Abs(WeaponHitbox.transform.localPosition.x),
                    WeaponHitbox.transform.localPosition.y, WeaponHitbox.transform.localPosition.z);
                Context.Animator.Play(Down.name);
            }
                
        }
        
    }

    public override void ExitState()
    {
        base.ExitState();
        WeaponHitbox.gameObject.SetActive(false);
        
    }
    public override void UpdateState()
    {
        base.UpdateState();
        
        Vector2 moveInput = Context.PlayerInput.moveVector.normalized;
        Context.Rb.velocity = moveInput * Context.Stats.moveSpeed;
        
        if (StateUptime > AttackTime)
        {
            IsComplete = true;
        }
        
        
    }
}
