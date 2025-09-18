using UnityEngine;
using UnityEditor;
using System.IO;

public class ProjectSetup : EditorWindow
{
    [MenuItem("Tools/Setup/Create Default Folders")]
    public static void CreateDefaultFolders()
    {
        string[] folders = new string[]
        {
            "Assets/Scripts",
            "Assets/Scenes",
            "Assets/Prefabs",
            "Assets/Art",
            "Assets/Materials",
            "Assets/Animations",
            "Assets/Audio"
        };

        foreach (string folder in folders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                Directory.CreateDirectory(folder);
                Debug.Log("Created folder: " + folder);
            }
        }

        AssetDatabase.Refresh();
    }
}
