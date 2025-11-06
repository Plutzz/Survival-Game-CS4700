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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Inventory)
        {
            GameManager.Instance.ChangeGameState(GameState.Inventory);
            MoveSlotsTo(insideBar, outsideBar);
            cursor.SetActive(true);
        }
        else
        {
            GameManager.Instance.ChangeGameState(GameState.Gameplay);
            MoveSlotsTo(outsideBar, insideBar);
            cursor.SetActive(false);
        }
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
