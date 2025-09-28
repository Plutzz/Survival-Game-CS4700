using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;
using Unity.VisualScripting;

public class SelectedBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] options;
    [SerializeField] private float time = 0.5f;
    [SerializeField] private Ease ease;
    [SerializeField] private Color selectedColor, unselectedColor;
    [SerializeField] private UnityEvent OnSelect;
    // Start is called before the first frame update
    void Start()
    {
        MainMenuManager.Instance.optionIndex = 0;
        Debug.Log(options[MainMenuManager.Instance.optionIndex].transform.position.y);
        transform.position = new Vector3(transform.position.x,
            options[MainMenuManager.Instance.optionIndex].transform.position.y, transform.position.z);
        SetColor();
    }
    
    private void OnEnable()
    {
        MainMenuManager.Instance.optionIndex = 0;
        Debug.Log(options[MainMenuManager.Instance.optionIndex].transform.position.y);
        transform.position = new Vector3(transform.position.x,
            options[MainMenuManager.Instance.optionIndex].transform.position.y, transform.position.z);
        SetColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainMenuManager.Instance.isTyping) return;
        
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MainMenuManager.Instance.optionIndex = (MainMenuManager.Instance.optionIndex + 1) % options.Length;
            GoToNewOption();
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MainMenuManager.Instance.optionIndex = (MainMenuManager.Instance.optionIndex - 1 + options.Length) % options.Length;
            GoToNewOption();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            options[MainMenuManager.Instance.optionIndex].GetComponentInChildren<MMF_Player>()?.PlayFeedbacks();
            OnSelect?.Invoke();
        }
    }

    private void GoToNewOption()
    {
        DOTween.Kill(transform);
        SetColor();
        RectTransform rect = transform as RectTransform;
        rect.DOMoveY(options[MainMenuManager.Instance.optionIndex].transform.position.y, time, true).SetEase(ease);
        Debug.Log(options[MainMenuManager.Instance.optionIndex].transform.position.y);
        options[MainMenuManager.Instance.optionIndex].GetComponentInChildren<MMF_Player>()?.PlayFeedbacks();
        
    }

    private void ResetOptionFeedbacks()
    {
        foreach (var o in options)
        {
            o.GetComponentInChildren<MMF_Player>()?.ResetFeedbacks();
            o.transform.localScale = Vector3.one;
        }
    }

    private void SetColor()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].color = unselectedColor;
        }
        
        options[MainMenuManager.Instance.optionIndex].color = selectedColor;
    }


}
