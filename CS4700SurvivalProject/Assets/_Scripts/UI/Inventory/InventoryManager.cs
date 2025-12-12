using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    private ItemDictionary itemDictionary; //for later
    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public GameObject InventoryItemPrefab;
    [SerializeField] private InventoryCursorAnimation cursor;
    [SerializeField] private Transform inventorySlotsParent;
    [SerializeField] private InventorySlot[] hotbarSlots;
    [field: SerializeField] public InventoryItem heldItem { get; private set; }
    
    public Action OnHeldItemChanged;
    public Action<ItemSO> OnItemAdded;

    private InventorySlot _hoveredSlot;
    [Header("Tooltip Menu")]
    [SerializeField] private GameObject _tooltipMenu;
    [SerializeField] private TextMeshProUGUI _itemNameText, _itemDescriptionText;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Vector3 _tooltipOffset;
    int selectedSlot = -1;
    private void Start()
    {
        _tooltipMenu.SetActive(false);
        ChangeSelectedSlot(0);
    }

    public void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number <= hotbarSlots.Length)
            {
                ChangeSelectedSlot(number - 1);
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            int newValue = selectedSlot - (int)(scroll / Mathf.Abs(scroll));
            if (newValue < 0)
            {
                newValue = inventorySlots.Length - 1;
            }
            else if (newValue >= inventorySlots.Length)
            {
                newValue = 0;
            }
            ChangeSelectedSlot(newValue % hotbarSlots.Length);
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        
        cursor.MoveToPosition(hotbarSlots[newValue].transform);
        InventoryItem inventoryItem = hotbarSlots[newValue].GetComponentInChildren<InventoryItem>();
        if (inventoryItem != null && inventoryItem.count <= 0)
        {
            heldItem = null;
        }
        else
        {
            heldItem = inventoryItem;
        }
        Debug.Log("Holding new item: " + inventoryItem);   
        selectedSlot = newValue;
        OnHeldItemChanged?.Invoke();
    }

    public void RemoveSelectedItem()
    {
        RemoveItem(hotbarSlots[selectedSlot]);
        ChangeSelectedSlot(selectedSlot);
    }

    public bool AddItem(ItemSO item)
    {
        //Check if any slot has the same item with count lower than max
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < maxStackedItems &&
                itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                OnItemAdded?.Invoke(item);
                ChangeSelectedSlot(selectedSlot);
                return true;
            }
        }
        //Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                OnItemAdded?.Invoke(item);
                ChangeSelectedSlot(selectedSlot);
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(InventorySlot slot, int amountToRemove = 1)
    {
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Debug.Log($"Removing {amountToRemove} from item: {itemInSlot.item} with count: {itemInSlot.count}");
            itemInSlot.count -= amountToRemove;
            if (itemInSlot.count <= 0)
            {
                Destroy(itemInSlot.gameObject);
            }
            else
            {
                itemInSlot.RefreshCount();
            }
        }
    }
    void SpawnNewItem(ItemSO item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(InventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public ItemSO GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            ItemSO item = itemInSlot.item;
            if (use)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        return null;
    }


    public void ShowTooltip(InventorySlot slot)
    {
        // Populate and show menu
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot == null) return;
        
        _itemNameText.text = itemInSlot.item.name;
        _itemDescriptionText.text = "Test description";
        _itemImage.sprite = itemInSlot.item.worldSprite;
        
        _hoveredSlot = slot;
        
        Vector3 offset = slot.transform.position.x < Screen.width / 2f ? _tooltipOffset : -_tooltipOffset;
        _tooltipMenu.transform.position = slot.transform.position + offset;
        _tooltipMenu.SetActive(true);
    }

    public void HideTooltip(InventorySlot slot)
    {
        if (slot != _hoveredSlot) return;
        
        _hoveredSlot = null;
        _tooltipMenu.SetActive(false);
        
    }

}
