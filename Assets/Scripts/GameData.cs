using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Stores the data about player and enemy prefabs
/// </summary>
[CreateAssetMenu(fileName = "Game Data", menuName = "Assets/Game Data")]
public class GameData : ScriptableObject
{
    [Header("PLAYER'S PARAMS")]
    [SerializeField] private string playerName;
    [SerializeField] private int playerMaxHP;
    [SerializeField] private float playerSpeed;

    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private EnemyParams[] enemyParams;

    public EnemyParams GetEnemyParamsByName(string name)
    {
        for (int i = 0; i < enemyParams.Length; i++)
        {
            if (name == enemyParams[i].Id)
                return enemyParams[i];
        }
        return null;
    }

    public GameObject GetPrefabByName(string name)
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (name == enemyPrefabs[i].name)
                return enemyPrefabs[i];
        }
        return null;
    }
}

[Serializable]
public class EnemyParams
{
    public string Id;
    public int HealthPoints;
    public float Speed;
}
