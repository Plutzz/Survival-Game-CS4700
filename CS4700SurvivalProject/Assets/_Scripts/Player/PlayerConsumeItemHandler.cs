using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerConsumeItemHandler : NetworkBehaviour
{
    private Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    
    private void Update()
    {
        if (!IsOwner) return;

        if (GameManager.Instance.CurrentGameState != GameState.Gameplay) return;

        if (Input.GetMouseButtonDown(1))
        {
            InventoryItem item = InventoryManager.Instance.heldItem;
            
            if (item == null)
            {
                Debug.LogError("No Item Held");
                return;
            }

            if (item.item.type == ItemType.Consumable)
            {
                
                foreach (ScriptableObject effectSO in item.item.effects)
                {
                    if(effectSO is IItemEffect itemEffect)
                        itemEffect.Apply(player);
                }
                
                InventoryManager.Instance.RemoveSelectedItem();
            }
            
        }
    }
}