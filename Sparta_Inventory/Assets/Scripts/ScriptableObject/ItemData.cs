using System;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable
}

public enum EquipableType
{
    Weapon,
    Armor
}

public enum ConsumableType
{
    Health,
    Deffence
}

[Serializable]
public class ItemDataEquipable
{
    public EquipableType equipableType;
    public float value;
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType consumableType;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string displayName;
    public string description;
    public ItemType itemType;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;
    public int quantity;

    [Header("Equip State")]
    public bool isEquipped = false;

    [Header("Type Data")]
    public ItemDataEquipable equipableData;
    public ItemDataConsumable consumableData;
}
