using UnityEngine;
using System.Collections;
using WorldTime;

public class NightWaveSpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Wave Settings")]
    [SerializeField] private int wavesPerNight = 3;
    [SerializeField] private float waveInterval = 5f;
    [SerializeField] private int enemiesPerWave = 3;
    [SerializeField] private float spawnInterval = 0.6f;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private WorldTime.WorldTime worldTime;
    [SerializeField] private WorldTime.WorldBiomeChange biomeManager;
    [SerializeField] private BossManager bossManager;

    private void OnEnable()
    {
        biomeManager.BossNight += OnBossNight;
        biomeManager.NormalNight += OnNormalNight;
    }

    private int DayInCurrentBiome
    {
        get
        {
            return ((worldTime.CurrentDay - 1) % biomeManager.daysPerBiome) + 1;
        }
    }

    private void OnDisable()
    {
        biomeManager.BossNight -= OnBossNight;
        biomeManager.NormalNight -= OnNormalNight;
    }

    private void OnBossNight()
    {
        bossManager.SpawnBoss();
    }

    private void OnNormalNight()
    {
        StartCoroutine(SpawnNightWaves());
    }

    private IEnumerator SpawnNightWaves()
    {
        int totalWaves = wavesPerNight + worldTime.CurrentDay / 2;

        for (int wave = 0; wave < totalWaves; wave++)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            if (wave < totalWaves - 1)
                yield return new WaitForSeconds(waveInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0)
            return;

        GameObject prefab = GetWeightedRandomEnemy();
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(prefab, point.position, Quaternion.identity);
    }

    private GameObject GetWeightedRandomEnemy()
    {
        int currentBiomeDay = DayInCurrentBiome;
        var availableEnemies = new System.Collections.Generic.List<GameObject>();
        foreach (var prefab in enemyPrefabs)
        {
            Enemy enemy = prefab.GetComponent<Enemy>();
            if (enemy != null && currentBiomeDay >= enemy.startDay)
            {
                availableEnemies.Add(prefab);
            }
        }

        if (availableEnemies.Count == 0)
            return enemyPrefabs[0];

        float totalWeight = 0f;
        foreach (var prefab in availableEnemies)
        {
            Enemy enemy = prefab.GetComponent<Enemy>();
            float adjustedWeight = enemy.weight * (1 + 0.1f * (currentBiomeDay - enemy.startDay));
            totalWeight += adjustedWeight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float current = 0f;

        foreach (var prefab in availableEnemies)
        {
            Enemy enemy = prefab.GetComponent<Enemy>();
            float adjustedWeight = enemy.weight * (1 + 0.1f * (currentBiomeDay - enemy.startDay));
            current += adjustedWeight;
            if (randomValue <= current)
                return prefab;
        }

        return availableEnemies[0];
    }
}
