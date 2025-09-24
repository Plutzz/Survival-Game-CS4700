using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenuManager : Singleton<PauseMenuManager>
{
    [SerializeField] private UnityEvent onPause, onUnpause;
    [SerializeField] private GameObject pauseMenuCanvas;
    [HideInInspector] public bool isGamePaused = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    
    void PauseGame()
    {
        isGamePaused = true;
        pauseMenuCanvas.SetActive(true);
        onPause?.Invoke();
    }

    void UnpauseGame()
    {
        isGamePaused = false;
        pauseMenuCanvas.SetActive(false);
        onUnpause?.Invoke();
    }
}
