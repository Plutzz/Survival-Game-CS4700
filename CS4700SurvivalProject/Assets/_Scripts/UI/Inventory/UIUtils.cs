using UnityEngine;
using UnityEngine.EventSystems;

public static class UIUtils
{
    // Clears the current selection in the EventSystem to avoid stuck UI states
    public static void ClearSelection()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}

