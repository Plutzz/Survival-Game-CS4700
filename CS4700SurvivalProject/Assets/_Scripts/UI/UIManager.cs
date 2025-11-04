using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [field: Header("In Game HUD")]
    [field: SerializeField] public GameObject InGameHUDMenu { get; private set; }
    [field: SerializeField] public MMProgressBar HealthBar { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetHealthBar(int value, int maxValue)
    {
        HealthBar.UpdateBar(value, 0, maxValue);
    }
}
