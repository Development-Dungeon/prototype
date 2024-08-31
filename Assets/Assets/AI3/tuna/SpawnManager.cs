using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Serializable]
    public class PrefabLimit
    {
        public GameObject prefab;
        public int MaxSpawn;
        public List<GameObject> spawns = new();
        public bool IsMaxedSpawned() => spawns?.Count >= MaxSpawn;
    }

    public List<PrefabLimit> spawnMetadata;
    public List<GameObject> spawnLocations;
    public int SpawnCoolDownInSeconds = 10;
    public GameObject parent;
    private Utilities.CountdownTimer SpawnCooldownTimer;

    void Start()
    {
        SpawnCooldownTimer = new Utilities.CountdownTimer(SpawnCoolDownInSeconds);
        SpawnCooldownTimer.OnTimerStop += SpawnEnemy;
        SpawnCooldownTimer.Start();
    }

    private void SpawnEnemy()
    {
        SpawnCooldownTimer.Reset(SpawnCoolDownInSeconds);
        SpawnCooldownTimer.Start();

        if (spawnMetadata == null)
            return;
        // go down the list and spawn the next item in the area 
        // find first entry that is not at the limit
        var prefabLimit = spawnMetadata.Find((metadata) => !metadata.IsMaxedSpawned());

        if (prefabLimit == null)
            return;

	    var randomLocationIndex = UnityEngine.Random.Range(0, spawnLocations.Count);
	    var spawnTransform = spawnLocations[randomLocationIndex].transform;
	    var newCreature = Instantiate(prefabLimit.prefab, spawnTransform.position, spawnTransform.rotation, parent.transform);
	    prefabLimit.spawns.Add(newCreature);


    }

    void Update()
    {
        SpawnCooldownTimer.Tick(Time.deltaTime);
    }
}
