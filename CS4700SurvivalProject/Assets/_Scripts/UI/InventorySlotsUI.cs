using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotsUI : MonoBehaviour
{
    [SerializeField] private Transform[] inventorySlots;
    [SerializeField] private Transform inventorySlotsParent;
    [SerializeField] private InventoryCursorAnimation cursor;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float scrollTime;
    [SerializeField] private Ease scrollEase;
    private int currentInventorySlot = 0;
    // Start is called before the first frame update
    void Start()
    {
        MoveToInventorySlot(inventorySlots[currentInventorySlot]);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(0,0);

        if (Input.GetKeyDown(KeyCode.D))
        {
            input += Vector2.right;    
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            input += Vector2.left;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            input += Vector2.up;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            input += Vector2.down;
        }
        
        
        if (input.x > 0 && currentInventorySlot < inventorySlots.Length - 1)
        {
            currentInventorySlot++;
            MoveToInventorySlot(inventorySlots[currentInventorySlot]);
        }

        if (input.x < 0 && currentInventorySlot > 0)
        {
            currentInventorySlot--;
            MoveToInventorySlot(inventorySlots[currentInventorySlot]);
        }
        
        
        if (input.y > 0 && currentInventorySlot > 0)
        {
            currentInventorySlot = Mathf.Clamp(currentInventorySlot - 3, 0,inventorySlots.Length - 1);
            MoveToInventorySlot(inventorySlots[currentInventorySlot]);
        }

        if (input.y < 0 && currentInventorySlot < inventorySlots.Length)
        {
            currentInventorySlot = Mathf.Clamp(currentInventorySlot + 3, 0,inventorySlots.Length - 1);
            MoveToInventorySlot(inventorySlots[currentInventorySlot]);
        }
    }

    private void MoveToInventorySlot(Transform inventorySlot)
    {
        DOTween.Kill(transform);
        scrollRect.DOVerticalNormalizedPos(
            ((float)(inventorySlots.Length - currentInventorySlot + 1) / 3) / (float)(inventorySlots.Length / 3),
            scrollTime);
        cursor.MoveToPosition(inventorySlot);
    }
}
