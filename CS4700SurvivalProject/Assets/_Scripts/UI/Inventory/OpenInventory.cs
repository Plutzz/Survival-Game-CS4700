using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class OpenInventory : MonoBehaviour
{
    [SerializeField] GameObject mainInventory;
    [SerializeField] GameObject toolBar;
    [FormerlySerializedAs("cursor")] [SerializeField] private GameObject hotbarCursor;
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
            hotbarCursor.SetActive(false);
        }
        else
        {
            GameManager.Instance.ChangeGameState(GameState.Gameplay);
            MoveSlotsTo(outsideBar, insideBar);
            hotbarCursor.SetActive(true);
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
