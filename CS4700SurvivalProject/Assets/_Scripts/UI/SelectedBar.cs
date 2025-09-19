using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;

public class SelectedBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] options;
    [SerializeField] private float time = 0.5f;
    [SerializeField] private Ease ease;
    [SerializeField] private Color selectedColor, unselectedColor;
    private int optionIndex;
    // Start is called before the first frame update
    void Start()
    {
        SetColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            optionIndex = (optionIndex + 1) % options.Length;
            GoToNewOption();
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            optionIndex = (optionIndex - 1 + options.Length) % options.Length;
            GoToNewOption();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            MainMenuManager.Instance.SelectOption(optionIndex);
        }
    }

    private void GoToNewOption()
    {
        DOTween.Kill(transform);
        SetColor();
        RectTransform rect = transform as RectTransform;
        rect.DOMoveY(options[optionIndex].transform.position.y, time, true).SetEase(ease);
        
        options[optionIndex].GetComponentInChildren<MMF_Player>()?.PlayFeedbacks();
        
    }

    private void SetColor()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].color = unselectedColor;
        }
        
        options[optionIndex].color = selectedColor;
    }
}
