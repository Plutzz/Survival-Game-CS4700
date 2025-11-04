using System;
using System.Collections;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[Serializable]
public enum GameState
{
    Title,
    Loading,
    Gameplay,
    Dialogue,
    Paused,
    Cutscene
}

public class GameManager : Singleton<GameManager>
{
    [field: SerializeField] public SceneField MainMenuScene { get; private set; }
    [field: SerializeField, ReadOnly] public GameState CurrentGameState { get; private set; }
    
    [SerializeField] private MMF_Player sceneLoader;
    
    
    
    /// <summary>
    /// Action that is invoked when the game state is changed.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item><description><c>GameState newState</c>: The state that was changed to.</description></item>
    /// </list>
    /// </remarks>
    public event Action<GameState> OnGameStateChanged = delegate { };

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == MainMenuScene.SceneName)
        {
            ChangeGameState(GameState.Title, true);
        }
        else
        {
            ChangeGameState(GameState.Gameplay, true);
        }
    }
    /// <summary>
    /// Changes the current game state to the specified new state.
    /// Will not change if the new state is the same as the current state unless 'force' is true.
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="force">Whether to rechange to the same state.</param>
    public void ChangeGameState(GameState newState, bool force = false)
    {
        if(CurrentGameState == newState && !force)
            return;

        CurrentGameState = newState;
        OnGameStateEnter(newState);

        OnGameStateChanged.Invoke(newState);
    }

    public void ChangeGameState(int newState)
    {
        ChangeGameState((GameState)newState, false);
    }
    
    public void ChangeGameState(GameState newState)
    {
        ChangeGameState(newState, false);
    }

    /// <summary>
    /// Called when after the game state has changed.
    /// Handle any UI updates or game logic that should occur when entering a new game state.
    /// </summary>
    /// <param name="newState"></param>
    private void OnGameStateEnter(GameState newState)
    {
        switch (newState)
        {
            case GameState.Title:
                Time.timeScale = 1f;
                UIManager.Instance.HideAllMenus();
                UIManager.Instance.ShowHUD(false);
                InputManager.Instance.EnableUIInput();
                InputManager.Instance.LockCursor(false);
                // AudioManager.Instance.PlaySong(AudioManager.Songs.MenuSong);
                break;
            case GameState.Loading:
                Time.timeScale = 0f;
                UIManager.Instance.ShowHUD(false);
                // AudioManager.Instance.StopSong();
                InputManager.Instance.DisableAllInput();
                InputManager.Instance.LockCursor(false);
                break;
            case GameState.Gameplay:
                Time.timeScale = 1f;
                UIManager.Instance.HideAllMenus();
                UIManager.Instance.ShowHUD(true);
                InputManager.Instance.EnablePlayerInput();
                InputManager.Instance.LockCursor(false);
                break;
            case GameState.Dialogue:
                Time.timeScale = 1f;
                UIManager.Instance.ShowHUD(true);
                InputManager.Instance.LockCursor(false);
                InputManager.Instance.EnableUIInput();
                // Show dialogue panel
                
                
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                // UIManager.Instance.OpenMenu(UIManager.Instance.PauseMenu);
                UIManager.Instance.ShowHUD(false);
                InputManager.Instance.LockCursor(false);
                InputManager.Instance.EnableUIInput();
                break;
            case GameState.Cutscene:
                Time.timeScale = 1f;
                InputManager.Instance.LockCursor(true);
                UIManager.Instance.ShowHUD(false);
                InputManager.Instance.DisableAllInput();
                break;
        }
    }

    /// <summary>
    /// Asynchronously switches to the specified scene and changes the game state afterwards.
    /// While waiting for the scene to load, the game state is set to Loading.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="afterState"></param>
    /// <returns></returns>
    public void LoadScene(String sceneName, GameState afterState)
    {
        StartCoroutine(PlayFeedbackAndSwitch(sceneName, afterState));
    }
    
    private IEnumerator PlayFeedbackAndSwitch(String sceneName, GameState afterState)
    {
        bool done = false;
        ChangeGameState(GameState.Loading);
        sceneLoader.GetFeedbackOfType<MMF_LoadScene>().DestinationSceneName = sceneName;
        sceneLoader.Events.OnComplete.AddListener(() => done = true);
        sceneLoader.PlayFeedbacks();
        yield return new WaitForSecondsRealtime(0.1f);
        UIManager.Instance.HideAllMenus();
        
        yield return new WaitUntil(() => done);

        // Unsubscribe to prevent memory leaks
        sceneLoader.Events.OnComplete.RemoveAllListeners();
        
        ChangeGameState(afterState);
    }

    public void ResumeGame()
    {
        ChangeGameState(GameState.Gameplay);
    }

    public void ReturnToMainMenu()
    {
        LoadScene(MainMenuScene.SceneName, GameState.Title);
    }

    /// <summary>
    /// Quits the game properly based on the platform.
    /// </summary>
    public static void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
    