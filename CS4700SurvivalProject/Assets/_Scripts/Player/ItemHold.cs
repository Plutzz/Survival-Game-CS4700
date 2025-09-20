using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemHold : MonoBehaviour
{
    [SerializeField] private ItemHoldPosition up, down, left, right;
    [SerializeField] private Player player;
    [SerializeField] private DynamicYSort playerYSort;
    [FormerlySerializedAs("itemSprite")] [SerializeField] private SpriteRenderer itemSpriteRenderer;
    private ItemHoldPosition currentPosition;
    private void Update()
    {
        if (player.stateMachine.currentState is PlayerAttack) return;
        
        
        if (Mathf.Abs(player.lookDir.x) > Mathf.Abs(player.lookDir.y))
        {

            if (player.lookDir.x > 0)
                SetItemPosition(right);
            else
                SetItemPosition(left);

        }
        else
        {
            if (player.lookDir.y > 0)
                SetItemPosition(up);
            else
                SetItemPosition(down);
        }
    }

    private void SetItemPosition(ItemHoldPosition position)
    {
        Debug.Log("SetItemPosition");
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
