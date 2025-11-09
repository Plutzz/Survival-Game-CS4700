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

    private void OnEnable()
    {
        InventoryManager.Instance.OnHeldItemChanged += ChangeItem;
    }

    private void OnDisable()
    {
        InventoryManager.Instance.OnHeldItemChanged -= ChangeItem;
    }

    private void ChangeItem()
    {
        if (InventoryManager.Instance.heldItem == null)
        {
            itemSpriteRenderer.sprite = null;
        }
        else if (InventoryManager.Instance.heldItem.heldSprite != null)
        {
            itemSpriteRenderer.sprite = InventoryManager.Instance.heldItem.heldSprite;
            itemSpriteRenderer.transform.localScale = Vector3.one * InventoryManager.Instance.heldItem.heldScale;
        }
        else if(InventoryManager.Instance.heldItem.worldSprite != null)
        {
            itemSpriteRenderer.sprite = InventoryManager.Instance.heldItem.worldSprite;
            itemSpriteRenderer.transform.localScale = Vector3.one * InventoryManager.Instance.heldItem.heldScale;
        }
        else
        {
            Debug.LogError("Held item has no sprite");
        }
        
    }
    private void Update()
    {
        if (IsOwner && player.StateMachine.CurrentState is PlayerAttack) return;
        
        
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
