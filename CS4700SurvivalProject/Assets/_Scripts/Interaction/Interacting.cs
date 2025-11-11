using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interacting : InteractableBase {

    public static Boolean interacted = false;
    // public override void Interact() {
    //     Debug.Log("E has been pressed.");
    // }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.transform.CompareTag("Player")) {  
            InteractText.instance?.EnableInteractPrompt();
            Interactable = true;
            Debug.Log("Player entered");
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.transform.CompareTag("Player")) {
            InteractText.instance?.DisableInteractPrompt();
            Interactable = false;
            Debug.Log("Player exited");
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            Interaction = true;
        } else {
            Interaction = false;
        }

        if(Interaction && Interactable) {
            // Interact();
            interacted = true;
            Debug.Log("Interacted");
        } else {
            interacted = false;
        }
    }

}