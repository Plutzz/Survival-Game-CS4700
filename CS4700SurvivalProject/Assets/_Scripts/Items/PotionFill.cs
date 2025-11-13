using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionFill : MonoBehaviour
{
    public bool isWater;
    public bool isMud;
    public bool isSaltWater;
    public bool isMob;
    
    [Header("Potion References")]
    public ItemSO waterPotion;
    public ItemSO mudPotion;
    public ItemSO saltWaterPotion;
    public ItemSO mobPotion;
    
    void Update()
    {
        if (Interacting.interacted)
        {
            Debug.Log("Potion filled");
            InventoryManager.Instance.RemoveSelectedItem();
            
            if (isWater && waterPotion != null)
            {
                InventoryManager.Instance.AddItem(waterPotion);
            }
            else if (isMud && mudPotion != null)
            {
                InventoryManager.Instance.AddItem(mudPotion);
            }
            else if (isSaltWater && saltWaterPotion != null)
            {
                InventoryManager.Instance.AddItem(saltWaterPotion);
            }
            else if (isMob && mobPotion != null)
            {
                InventoryManager.Instance.AddItem(mobPotion);
            }
        }
    }
}