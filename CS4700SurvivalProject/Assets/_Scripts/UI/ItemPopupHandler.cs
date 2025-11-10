using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopupHandler : MonoBehaviour
{
    [SerializeField] private GameObject itemPopup;
    [SerializeField] private Transform itemPopupParent;

    private void OnEnable()
    {
        InventoryManager.Instance.OnItemAdded += DisplayItem;
    }

    private void DisplayItem(ItemSO item)
    {
        ItemPopupFade fade = Instantiate(itemPopup, itemPopupParent).GetComponent<ItemPopupFade>();
        fade.DisplayItem(item);
    }
}
