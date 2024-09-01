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
    private int lastSpawnIndex = 0;

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

        if (this.spawnMetadata == null)
            return;

        // find first entry that is not at the limit
        var foundNewIndex = false;
        for (int i = 1; i <= this.spawnMetadata.Count; ++i)
        {
            // check every index and see if it has an available max
            var indexMod = (lastSpawnIndex + i) % this.spawnMetadata.Count;
            if (!this.spawnMetadata[indexMod].IsMaxedSpawned())
            {
                lastSpawnIndex = indexMod;
                foundNewIndex = true;
		    }
        }

        if (!foundNewIndex) return;

        var prefabLimit = this.spawnMetadata[lastSpawnIndex];

        if (prefabLimit == null) return;

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
