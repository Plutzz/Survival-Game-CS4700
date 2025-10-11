using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    [SerializeField] GameObject mainInventory;
    [SerializeField] GameObject toolBar;
    [SerializeField] private GameObject cursor;
    public Transform outsideBar;
    public Transform insideBar;

    public static bool InventoryOpen { get; private set; } = false;
    void Start()
    {
        if (mainInventory != null)
            InventoryOpen = mainInventory.activeSelf;
        if (toolBar != null)
            toolBar.SetActive(!InventoryOpen);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && mainInventory.activeSelf)
        {
            CloseInventory();
        }
    }

    public void ToggleInventory()
    {
        InventoryOpen = !InventoryOpen;
        mainInventory.SetActive(InventoryOpen);

        if (InventoryOpen)
        {
            MoveSlotsTo(insideBar, outsideBar);
            cursor.SetActive(false);
        }

        else
        {
            MoveSlotsTo(outsideBar, insideBar);
            cursor.SetActive(true);
        }

        Debug.Log("Inventory Open: " + InventoryOpen);
    }

    public void CloseInventory()
    {
        InventoryOpen = false;
        mainInventory.SetActive(false);
        MoveSlotsTo(insideBar, outsideBar);
    }

    private void MoveSlotsTo(Transform newParent, Transform oldParent)
    {
        // Copy children into a list first so we don't modify the collection while iterating
        List<Transform> children = new List<Transform>();
        foreach (Transform child in oldParent)
            children.Add(child);

        foreach (Transform child in children)
            child.SetParent(newParent, false);
    }

}
