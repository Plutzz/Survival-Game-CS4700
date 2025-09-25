using MoreMountains.Feedbacks;
using UnityEngine;
using TMPro;

public class MenuTextInput : MonoBehaviour
{
    public TMP_Text inputDisplay;   // Assign in Inspector
    [SerializeField] private string promptText = "";
    public string currentInput = "";
    
    private float blinkTime = 0.5f;
    private float timer;
    private bool showCursor;

    void Start()
    {
        enabled = false;
    }
    void Update()
    {
        if (!MainMenuManager.Instance.isTyping)
        {
            inputDisplay.text = promptText + currentInput;
            return;
        }

        timer += Time.deltaTime;
        if(timer >= blinkTime)
        {
            showCursor = !showCursor;
            timer = 0f;
        }

        foreach (char c in Input.inputString)
        {
            if (c == '\b') // backspace
            {
                if (currentInput.Length > 0)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
            else if (c == '\n' || c == '\r') // return/enter
            {
                MainMenuManager.Instance.StopTyping();
                GetComponentInChildren<MMF_Player>()?.PlayFeedbacks();
                Debug.Log("Final Input: " + currentInput);
                enabled = false;
            }
            else
            {
                currentInput += char.ToUpper(c);;
            }

        }

        inputDisplay.text = promptText + currentInput + (showCursor ? "|" : "");
    }
}