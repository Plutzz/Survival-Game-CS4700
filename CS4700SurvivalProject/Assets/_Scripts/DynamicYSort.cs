using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicYSort : MonoBehaviour
{
    private int _baseSortingOrder;
    [SerializeField] private SortableSprite[] _sortableSprites;

    // Update is called once per frame
    void Update()
    {
        _baseSortingOrder = -(int)(transform.position.y * 100);
        foreach (var sortableSprite in _sortableSprites)
        {
            sortableSprite.spriteRenderer.sortingOrder = _baseSortingOrder + sortableSprite.relativeOrder;
        }
    }

    public void SetSortingOrder(int sortingOrder, SpriteRenderer sr)
    {
        for(int i = 0; i < _sortableSprites.Length; i++)
        {
            if (_sortableSprites[i].spriteRenderer == sr)
            {
                _sortableSprites[i] = new SortableSprite
                {
                    spriteRenderer = sr,
                    relativeOrder = sortingOrder
                };
                return;
            }
        }
    }

    [Serializable]
    public struct SortableSprite
    {
        public SpriteRenderer spriteRenderer;
        public int relativeOrder;
    }
}
