using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [SerializeField] private MMF_Player loadScenePlayer;
    [SerializeField] private GameObject[] menus;
    [SerializeField] private MenuTextInput joinCodeTextInput;
    public int optionIndex;
    [HideInInspector] public bool isTyping { get; private set; } = false;
    public void StartSelectOption()
    {
        switch (optionIndex)
        {
            // Start Game
            case 0:
                OpenMenu(1);
                break;
            // Options
            case 1:
                break;
            // Exit Game
            case 2:
                Application.Quit();
                break;
        }
    }
    
    public void JoinHostSelectOption()
    {
        switch (optionIndex)
        {
            // New Game
            case 0:
                OpenMenu(2);
                break;
            // Load Game
            case 1:
                OpenMenu(2);
                break;
            // Join Game
            case 2:
                OpenMenu(3);
                break;
            // Back to start
            case 3:
                OpenMenu(0);
                break;
        }
    }

    public void NewGameSelectOptions()
    {
        switch (optionIndex)
        {
            // Save1
            case 0:
                loadScenePlayer.PlayFeedbacks();
                break;
            // Save2
            case 1:
                loadScenePlayer.PlayFeedbacks();
                break;
            // Save3
            case 2:
                loadScenePlayer.PlayFeedbacks();
                break;
            // Back to Join/Host
            case 3:
                OpenMenu(1);
                break;
        }
    }
    
    public void JoinGameSelectOption()
    {
        switch (optionIndex)
        {
            // Join Code
            case 0:
                StartTyping();
                joinCodeTextInput.enabled = true;
                break;
            // Save2
            case 1:
                loadScenePlayer.PlayFeedbacks();
                break;
            // Back to Join/Host
            case 2:
                OpenMenu(1);
                break;
        }
    }

    private void OpenMenu(int index)
    {
        foreach(GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        
        menus[index].SetActive(true);
    }

    public void StartTyping()
    {
        isTyping = true;
    }

    public void StopTyping()
    {
        isTyping = false;
    }
}
