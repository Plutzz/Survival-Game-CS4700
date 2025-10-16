using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [SerializeField] private MMF_Player loadScenePlayer;
    [SerializeField] private GameObject[] menus;
    [SerializeField] private MenuTextInput joinCodeTextInput;
    [SerializeField] private SceneField gameScene;
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
                OpenMenu(4);
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
                HostGame();
                break;
            // Save2
            case 1:
                HostGame();
                break;
            // Save3
            case 2:
                HostGame();
                break;
            // Back to Join/Host
            case 3:
                OpenMenu(1);
                break;
        }
    }
    
    public async void JoinGameSelectOption()
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
                bool joined = await RelayManager.Instance.JoinRelay(joinCodeTextInput.currentInput);
                if (joined)
                {
                    LoadGameScene();
                }
                else
                {
                    // TODO: Show error feedback (e.g., flash red text, play error sound)
                    Debug.Log("Invalid code entered!");
                }
                break;
            // Back to Join/Host
            case 2:
                OpenMenu(1);
                break;
        }
    }
    
    public void SettingsMenuSelectOptions()
    {
        switch (optionIndex)
        {
            // Video Settings
            case 0:
                OpenMenu(5);
                break;
            // Audio Settings
            case 1:
                OpenMenu(6);
                break;
            // Gameplay Settings
            case 2:
                OpenMenu(7);
                break;
            // Back
            case 3:
                OpenMenu(0);
                break;
        }
    }
    public void VideoSettingsMenuSelectOptions()
    {
        switch (optionIndex)
        {
            // Back
            case 0:
                OpenMenu(4);
                break;
        }
    }
    public void AudioSettingsMenuSelectOptions()
    {
        switch (optionIndex)
        {
            // Back
            case 0:
                OpenMenu(4);
                break;
        }
    }
    public void GameplaySettingsMenuSelectOptions()
    {
        switch (optionIndex)
        {
            // Back
            case 0:
                OpenMenu(4);
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

    private async void HostGame()
    {
        string joinCode = await RelayManager.Instance.CreateRelay();
        if (joinCode != null)
        {
            // Show this on UI so your friend can enter it
            Debug.Log("Game started! Share join code: " + joinCode);
            LoadGameScene();
        }
        else
        {
            Debug.LogError("Something went wrong while hosting game");
        }
    }

    private void LoadGameScene()
    {
        Debug.Log(gameScene.SceneName);
        Debug.Log(NetworkManager.Singleton.SceneManager);
        NetworkManager.Singleton.SceneManager.LoadScene(gameScene.SceneName, LoadSceneMode.Single);
    }
}
