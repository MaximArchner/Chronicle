using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] playerStats; // [0] - Health, [1] - Energy/Hunger, [2] - Hydration/Thirst
    public float[] playerTransform; // [0] - positon x, y, z; [1] - rotation x, y, z;
    //public string[] inventoryContent;

    public PlayerData(float[] _playerStats, float[] _playerTransform)
    {
        playerStats = _playerStats;
        playerTransform = _playerTransform;
    }
}
