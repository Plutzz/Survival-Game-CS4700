using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeaponAttack : MonoBehaviour
{
    [SerializeField] private Ease ease;
    [SerializeField] private float distance = 0.5f, time = 0.5f;
    private float startPosition;

    private void Awake()
    {
        startPosition = transform.localPosition.y;
    }

    public void Attack()
    {
        DOTween.Kill(transform);
        transform.localPosition = new Vector3(transform.localPosition.x, startPosition, transform.localPosition.z);
        transform.DOLocalMoveY(startPosition - distance, time).SetEase(ease).SetLoops(2, LoopType.Yoyo);
    }
}
