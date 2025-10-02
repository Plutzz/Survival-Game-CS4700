using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InventoryCursorAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform menuParent;
    [SerializeField] private RectTransform cursorLogic, cursorGraphics;
    
    
    [Header("Breathing Animation")] 
    [SerializeField] private float time;
    [SerializeField] private Vector2 startSize, endSize;
    [SerializeField] private Ease ease;
    
    [Header("Moving Animation")]
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        DoBreatheAnimation();
    }
    
    void Update()
    {
        cursorGraphics.position = Vector3.Lerp(cursorGraphics.position, cursorLogic.position, moveSpeed * Time.deltaTime * 100f);
    }

    private void DoBreatheAnimation()
    {
        cursorGraphics.sizeDelta = startSize;
        cursorGraphics.DOSizeDelta(endSize, time).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }

    public void MoveToPosition(Transform inventorySlot, float delay = 0)
    {
        cursorLogic.parent = inventorySlot.transform;
        cursorLogic.localPosition = Vector3.zero;
        
    }
}
