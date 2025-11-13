using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public class Interacting : InteractableBase
{
    public bool canInteract;
    public static Boolean interacted = false;
    public bool inArea;

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check item in hand
        //check if player and if item in hand is interactable
        if (collider.transform.CompareTag("Player"))
        {
            inArea = true;
            Debug.Log("Player entered");
            CheckInteractable();
            if (canInteract)
            {
                InteractText.instance?.EnableInteractPrompt();
                Interactable = true;

            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            InteractText.instance?.DisableInteractPrompt();
            Interactable = false;
            inArea = false;
            Debug.Log("Player exited");
        }
    }

    void Update()
    {
        CheckInteractable();
        if (Interactable && !canInteract && inArea) 
        {
            InteractText.instance?.DisableInteractPrompt();
            Interactable = false;
            Debug.Log("reached");
        } else if (!Interactable && canInteract && inArea)
        {
            InteractText.instance?.EnableInteractPrompt();
            Interactable = true;
            Debug.Log("Player entered");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interaction = true;
        }
        else
        {
            Interaction = false;
        }

        if (Interaction && Interactable)
        {
            // Interact();
            interacted = true;
            Debug.Log("Interacted");
        }
        else
        {
            interacted = false;
        }
    }

    public void CheckInteractable()
    {
        // Use the getter to check which item is selected and being held
        InventoryItem heldInventoryItem = InventoryManager.Instance.heldItem;
        Debug.Log("Holding item: " + (heldInventoryItem != null ? "True" : "False"));

        if (heldInventoryItem != null && heldInventoryItem.item != null)
        {
            ItemSO heldItem = heldInventoryItem.item;
            Debug.Log($"Item Held: {heldItem.name} (type={heldItem.type}, id={heldItem.ID})");

            if (heldItem.type == ItemType.Fillable)
            {
                canInteract = true;
                Debug.Log("canInteract = true");
            }
            else
            {
                canInteract = false;
                Debug.Log("canInteract = false");
            }
        }
        else 
        {
            canInteract = false;
            Debug.Log("canInteract = false");
        }
    }
}