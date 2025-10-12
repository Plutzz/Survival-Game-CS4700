using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTabHandler : MonoBehaviour
{
    [Serializable]
    public struct MenuTab
    {
        public GameObject menu;
        public GameObject tabIcon;
    }
    
    [SerializeField] private MenuTab[] tabs;
    [SerializeField] private Color selectedTabColor, notSelectedTabColor;
    private int currentTab = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentTab = (currentTab + 1) % tabs.Length;
            UpdateTab();
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentTab--;
            if(currentTab < 0)
                currentTab =  tabs.Length - 1;
            
            UpdateTab();
        }
    }
    
    private void UpdateTab()
    {
        // Reset menu and tab
        for (int i = 0; i < tabs.Length; i++)
        {
            var tab = tabs[i];
            tab.menu.SetActive(false);
            if (i < currentTab)
            {
                tab.tabIcon.transform.SetSiblingIndex(i);
            }
            else
            {
                tab.tabIcon.transform.SetSiblingIndex(tabs.Length - i - 1);
            }
            tab.tabIcon.GetComponent<Image>().color = notSelectedTabColor;
        }

        tabs[currentTab].menu.SetActive(true);
        tabs[currentTab].tabIcon.transform.SetSiblingIndex(tabs.Length - 1);
        tabs[currentTab].tabIcon.GetComponent<Image>().color = selectedTabColor;
    }
}
