using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    [SerializeField] GameObject mainInventory;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool inventoryOpen = !mainInventory.activeSelf;
            Debug.Log("Inventory Open: " + inventoryOpen);
            mainInventory.SetActive(inventoryOpen);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && mainInventory.activeSelf)
        {
            mainInventory.SetActive(false);
        }
    }
}
