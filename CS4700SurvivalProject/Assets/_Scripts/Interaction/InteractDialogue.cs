using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

public class InteractDialogue : InteractableBase
{
    // Start is called before the first frame update
    public static Boolean interacted = false;

    public DialogueSequence sequence;

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.transform.CompareTag("Player")) {  
            InteractText.instance?.EnableInteractPrompt();
            Interactable = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.transform.CompareTag("Player")) {
            InteractText.instance?.DisableInteractPrompt();
            Interactable = false;
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            Interaction = true;
        } else {
            Interaction = false;
        }

        if(Interaction && Interactable) {
            sequence.StartSelf();
            gameObject.SetActive(false);
            interacted = true;
        } else {
            interacted = false;
        }
    }

}