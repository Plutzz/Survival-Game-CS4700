using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [field: SerializeField] public List<UnityEngine.GameObject> SubMenus { get; protected set; } = new List<UnityEngine.GameObject>();

    protected int CurrentSubMenu;
    void Start()
    {
        if(SubMenus.Count > 0)
            ChangeSubMenu(0);
    }
    
    public virtual void ChangeSubMenu(int index)
    {
        ChangeSubMenu(SubMenus[index]);
    }

    public virtual void ChangeSubMenu(UnityEngine.GameObject subMenu)
    {
        CurrentSubMenu = SubMenus.IndexOf(subMenu);
        DisableAllSubMenus();
        subMenu.SetActive(true);
    }

    protected void DisableAllSubMenus()
    {
        foreach (var menu in SubMenus)
        {
            menu.SetActive(false);
        }
    }
    
}
