using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Scriptable Item", menuName = "Item new/Create new Item")]
public class ItemNew: ScriptableObject
{

    public int id;
    public string itemName;

    [Header("Only gameplay")]
    public TileBase tile;
    public ItemType type;
    public ActionType actionType;
    public GameObject inWorldPrefab;

    [Header("Only UI")]
    public bool stackable = true;
    public int maxStackable = 10;


    [Header("Both")]
    public Sprite image;


}

public enum ItemType { 
    BuildingBlock,
    Tool
}

public enum ActionType {  
    None,
    Dig,
    Mine
}
