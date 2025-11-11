using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractText : MonoBehaviour {
    public TextMeshProUGUI interactText;
    public float enabledY;
    public float disabledY;
    public float tweenDuration;
    public Ease ease;
    public static InteractText instance;

    public void Awake() {
        if (instance != null) Destroy(this);
        instance = this;
    }

    public void EnableInteractPrompt() {
        interactText.rectTransform.DOMoveY(enabledY, tweenDuration).SetEase(ease);
    }

    public void DisableInteractPrompt() {
        interactText.rectTransform.DOMoveY(disabledY, tweenDuration).SetEase(ease);
    }
}