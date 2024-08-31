using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<GameObject> spawnLocations;
    protected List<GameObject> spawnedEnemies = new();
    public int MaxSpawn = 10;
    public int SpawnCoolDownInSeconds = 10;
    public GameObject parent;
    private Utilities.CountdownTimer SpawnCooldownTimer;

    // what do i want the spawn manager to do?
    // I want the spawn manager to every tick, randomly place a prefab in the world

    void Start()
    {
        SpawnCooldownTimer = new Utilities.CountdownTimer(SpawnCoolDownInSeconds);
        SpawnCooldownTimer.OnTimerStop += SpawnEnemy;
        SpawnCooldownTimer.Start();
        // when the cooldown when the cooldown timer is stoped then trigger a spawn event 
    }

    private void SpawnEnemy()
    {
        if (spawnedEnemies.Count < MaxSpawn)
        {
            var randomLocationIndex = UnityEngine.Random.Range(0, spawnLocations.Count);
            var spawnTransform = spawnLocations[randomLocationIndex].transform;
            var newCreature = Instantiate(enemyPrefab, spawnTransform.position, spawnTransform.rotation, parent.transform);
            spawnedEnemies.Add(newCreature);
        }

        SpawnCooldownTimer.Reset(SpawnCoolDownInSeconds);
        SpawnCooldownTimer.Start();
    }

    void Update()
    {
        SpawnCooldownTimer.Tick(Time.deltaTime);
    }
}
