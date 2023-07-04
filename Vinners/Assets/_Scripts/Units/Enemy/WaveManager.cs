using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FishNet.Object;

public sealed class WaveManager : NetworkBehaviour
{
    // Reference to the WaveData scriptable objects for the current wave
    public WaveData[] waveDatas;
    private WaveData currentWaveData;
    private int currentWaveIndex;
    private int enemyCountBuffer = 3;

    public static WaveManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartInitialWave();   
    }

    private void Update()
    {
        if (!IsServer) return;
        if (EnemyManager.Instance.GetEnemyDeathCount() >= currentWaveData.maxTotalEnemies - enemyCountBuffer)
        {
            StartNextWave();
        }     
    }

    public void StartInitialWave()
    {
        if (waveDatas != null && waveDatas.Length > 0)
        {
            Debug.Log("Activating all spawners");
            currentWaveIndex = 0;
            currentWaveData = waveDatas[0];
            ActivateAllSpawners();
        }
    }

    private void StartNextWave()
    {
        if (!IsServer) return;

        currentWaveIndex++;

        if (currentWaveIndex < waveDatas.Length)
        {
            //Reset Enemy Death Count
            EnemyManager.Instance.ResetDeathCount();

            // Set the current wave to the next wave in the list
            currentWaveData = waveDatas[currentWaveIndex];

            //Delay according to delay in current wave data.
            Invoke(nameof(ActivateAllSpawners), currentWaveData.waveDelay);
            Debug.Log("Next Wave Started");
        }
        else
        {
            // All waves have been spawned, end the game or restart from the beginning
            // Use Event. ie: public event OnWaveEnd()
            Debug.Log("Waves progression ended successfully");
        }
    }

    public void ActivateAllSpawners()
    {
        if (!IsServer) return;

        Debug.Log("Instantiate Spawners");
        foreach (GameObject enemySpawner in currentWaveData.enemySpawnerPrefabs)
        {
            var newSpawner = Instantiate(enemySpawner, enemySpawner.transform.position, Quaternion.identity);
            ServerManager.Spawn(newSpawner);
            newSpawner.GetComponent<EnemySpawner>().ActivateSpawnner();
            Debug.Log("Spawned a spawner");
        }
    }




    // The following methods are kept for testing purposes only
    // private List<GameObject> activeSpawners = new List<GameObject>();

    //public void SpawnerComplete(GameObject spawner)
    //{
    //    if (!IsServer) return;

    //    if (spawner.GetComponent<EnemySpawner>() != null)
    //    {
    //        activeSpawners.Remove(spawner);

    //        if (activeSpawners.Count <= 0 && EnemyManager.GetEnemyDeathCount() <= 0)
    //        {
    //            StartNextWave();
    //        }
    //    }
    //}

}

