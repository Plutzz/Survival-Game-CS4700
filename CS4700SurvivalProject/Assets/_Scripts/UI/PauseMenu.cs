using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PauseMenu : Menu
{
    [SerializeField] private TextMeshProUGUI joinCodeText;
    protected override void Start()
    {
        base.Start();
    }

    void OnEnable()
    {
        if (RelayManager.Instance != null)
            joinCodeText.text = "Join Code: " + RelayManager.Instance.joinCode;
    }
}
