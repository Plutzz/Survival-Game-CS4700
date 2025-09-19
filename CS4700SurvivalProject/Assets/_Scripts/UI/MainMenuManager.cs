using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [SerializeField] private MMF_Player loadScenePlayer;
    public void SelectOption(int optionIndex)
    {
        switch (optionIndex)
        {
            // Start Game
            case 0:
                loadScenePlayer.PlayFeedbacks();
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
}
