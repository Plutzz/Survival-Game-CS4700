using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InventoryCursorAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    
    [Header("Breathing Animation")] 
    [SerializeField] private float time;
    [SerializeField] private Vector2 startSize, endSize;
    [SerializeField] private Ease ease;
    
    [Header("Moving Animation")]
    [SerializeField] private float moveTime;
    [SerializeField] private Ease moveEase;
    // Start is called before the first frame update
    void Start()
    {
        DoBreatheAnimation();
    }

    private void DoBreatheAnimation()
    {
        rectTransform.sizeDelta = startSize;
        rectTransform.DOSizeDelta(endSize, time).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }

    public void MoveToPosition(Vector2 position)
    {
        transform.position = position;
        // transform.DOMove(position, moveTime).SetEase(moveEase);
    }
}
