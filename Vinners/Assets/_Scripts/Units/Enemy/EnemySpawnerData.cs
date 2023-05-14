using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerData", menuName = "EnemySpawnerData")]
public class EnemySpawnerData : ScriptableObject
{
    public GameObject[] enemyPrefabs;
    public float spawnRate = 2.0f;
    public Transform[] spawnLocations;
    public int maxEnemies;

}
