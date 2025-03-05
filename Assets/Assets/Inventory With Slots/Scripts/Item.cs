using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Scriptable Item", menuName = "Item new/Create new Item")]
public class Item: ScriptableObject
{
    public int id;
    public string itemName;

    [Header("Only gameplay")]
    public TileBase tile;
    public ItemType type;
    public ActionType actionType;
    public GameObject inWorldPrefab;
    public GameObject leftHandPrefab; 
    public float value;

    [Header("Player Equipement Attributes")]
    public int oxygenTankIncrease;
    public int depthPresureIncrease;
    public int healthIncrease;

    [Header("Weapon States")]
    public float damage;
    public float range;
    public float innerConeAngle;
    public float cooldownInSeconds;
    public bool canAttackMultiple;

    [Header("Only UI")]
    public bool stackable = true;
    public int maxStackable = 10;

    [Header("Both")]
    public Sprite image;

    [Header("Fuel Items")] 
    public float burnTimeInSeconds;
}

public enum ItemType { 
    BuildingBlock,
    Tool,
    Head,
    Chest,
    Suit,
    Tank,
    Money,
    Weapon,
    Misc,
    Fuel
}

public enum ActionType {  
    None,
    Dig,
    Mine,
    Attack
}
