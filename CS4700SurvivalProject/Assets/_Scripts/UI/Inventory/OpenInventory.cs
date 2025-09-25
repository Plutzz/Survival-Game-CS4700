using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    [SerializeField] GameObject mainInventory;
    public static bool InventoryOpen { get; private set; } = false;
    void Start()
    {
        if (mainInventory != null)
            InventoryOpen = mainInventory.activeSelf;
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
        InventoryOpen = !mainInventory.activeSelf;
        Debug.Log("Inventory Open: " + InventoryOpen);
        mainInventory.SetActive(InventoryOpen);
    }

    public void CloseInventory()
    {
        InventoryOpen = false;
        mainInventory.SetActive(false);
    }
}
