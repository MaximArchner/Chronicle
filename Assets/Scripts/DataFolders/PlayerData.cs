using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] playerStats; // [0] - Health, [1] - Energy/Hunger, [2] - Hydration/Thirst
    public float[] playerTransform; // [0] - positon x, y, z; [1] - rotation x, y, z;
    public List<InventoryItemData> inventoryContent;
    public string[] quickSlotContent;

    public PlayerData(float[] _playerStats, float[] _playerTransform, List<InventoryItemData> inventoryContent, string[] _quickSlotContent)
    {
        playerStats = _playerStats;
        playerTransform = _playerTransform;
        this.inventoryContent = inventoryContent;
        quickSlotContent = _quickSlotContent;
    }
}

[System.Serializable]
public class InventoryItemData
{
    public string itemName;
    public int quantity;

    public InventoryItemData(string itemName, int quantity)
    {
        this.itemName = itemName;
        this.quantity = quantity;
    }
}
