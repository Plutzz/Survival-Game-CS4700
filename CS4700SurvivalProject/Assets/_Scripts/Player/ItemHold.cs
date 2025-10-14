using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemHold : NetworkBehaviour
{
    [SerializeField] private ItemHoldPosition up, down, left, right;
    [SerializeField] private Player player;
    [SerializeField] private DynamicYSort playerYSort;
    [FormerlySerializedAs("itemSprite")] [SerializeField] private SpriteRenderer itemSpriteRenderer;
    private ItemHoldPosition currentPosition;
    private void Update()
    {
        if (IsOwner && player.CurrentState is PlayerAttack) return;
        
        
        if (Mathf.Abs(player.lookDir.Value.x) > Mathf.Abs(player.lookDir.Value.y))
        {

            if (player.lookDir.Value.x > 0)
                SetItemPosition(right);
            else
                SetItemPosition(left);

        }
        else
        {
            if (player.lookDir.Value.y > 0)
                SetItemPosition(up);
            else
                SetItemPosition(down);
        }
    }
    private void SetItemPosition(ItemHoldPosition position)
    {
        transform.localPosition = position.localPosition;
        transform.localEulerAngles = new Vector3(0, 0, position.rotation);
        itemSpriteRenderer.flipX = position.flipX;
        playerYSort.SetSortingOrder(position.relativeSpriteOrder, itemSpriteRenderer);
    }
}

[Serializable]
public struct ItemHoldPosition
{
    public Vector2 localPosition;
    public float rotation;
    public bool flipX;
    public int relativeSpriteOrder;
}
