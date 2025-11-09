using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class ItemSO : ScriptableObject
{
    public int ID;
    public TileBase tile;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [FormerlySerializedAs("image")] [Header("Sprites")]
    public Sprite worldSprite;
    public float worldScale = 1f;
    public Sprite heldSprite;
    public float heldScale = 1f;
}

public enum ItemType
{
    BuildingBlock,
    Tool
}

public enum ActionType
{
     Dig,
     Mine
}
