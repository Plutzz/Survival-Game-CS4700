using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    // [field: SerializeField] public Menu SettingsMenu { get; private set; }
    
    [field: Header("In Game HUD")]
    [field: SerializeField] public GameObject InGameHUD { get; private set; }
    [field: SerializeField] public MMProgressBar HealthBar { get; private set; }
    public void SetHealthBar(int value, int maxValue)
    {
        HealthBar.UpdateBar(value, 0, maxValue);
    }
    
    public void HideAllMenus()
    {
        InGameHUD.gameObject.SetActive(false);
    }

    public void ShowHUD(bool show)
    {
        InGameHUD.gameObject.SetActive(show);
    }
}
