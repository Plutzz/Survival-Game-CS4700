using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    public static CursorManager Instance;

    [System.Serializable]
    public class CursorData
    {
        public string toolName;          // e.g. "Axe"
        public Texture2D cursorTexture;  // Cursor image
        public Vector2 hotspot;          // Click hotspot (usually middle or top-left)
    }

    [SerializeField] private CursorData[] cursors;
    private string currentTool = "";
    [SerializeField] private string cursorType = "";

    void Update()
    {
        SetCursor(cursorType);
    }

    public void SetCursor(string toolName)
    {
        if (toolName == currentTool) return;

        Debug.Log("Switching Cursor");
        
        foreach (var data in cursors)
        {
            if (data.toolName == toolName)
            {
                Cursor.SetCursor(data.cursorTexture, data.hotspot, CursorMode.Auto);
                currentTool = toolName;
                return;
            }
        }

        // Default back to normal cursor if not found
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        currentTool = "";
    }
}