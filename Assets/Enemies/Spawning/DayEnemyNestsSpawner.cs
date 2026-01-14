using UnityEngine;
using System.Collections.Generic;

public class DayEnemyNestSpawner : MonoBehaviour
{
    [Header("Nest Settings")]
    [SerializeField] private GameObject nestPrefab;
    [SerializeField] private int minNestsPerDay = 3;
    [SerializeField] private int maxNestsPerDay = 4;

    [Header("Nest Spawn Points")]
    [SerializeField] private Transform[] nestSpawnPoints;

    [Header("Chest Settings")]
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private int minChestsPerDay = 1;
    [SerializeField] private int maxChestsPerDay = 2;

    [Header("Chest Spawn Points")]
    [SerializeField] private Transform[] chestSpawnPoints;

    [Header("Spawn Offset Radius")]
    [SerializeField] private float spawnRadius = 1.5f;

    [SerializeField] private WorldTime.WorldTime worldTime;

    private List<GameObject> spawnedChests = new List<GameObject>();

    private void OnEnable()
    {
        if (worldTime != null)
            worldTime.DayChanged += OnDayChanged;
    }

    private void OnDisable()
    {
        if (worldTime != null)
            worldTime.DayChanged -= OnDayChanged;
    }

    private void OnDayChanged()
    {
        if (!gameObject.activeInHierarchy)
            return;

        ClearChests();

        SpawnNests();
        SpawnChests();
    }

    private void SpawnNests()
    {
        int nestsToSpawn = Random.Range(minNestsPerDay, maxNestsPerDay + 1);

        List<Transform> availablePoints = new List<Transform>(nestSpawnPoints);

        for (int i = 0; i < nestsToSpawn && availablePoints.Count > 0; i++)
        {
            int index = Random.Range(0, availablePoints.Count);
            Transform point = availablePoints[index];
            availablePoints.RemoveAt(index);

            Vector3 spawnPos = point.position + (Vector3)Random.insideUnitCircle * spawnRadius;
            GameObject nest = Instantiate(nestPrefab, spawnPos, Quaternion.identity, transform);
        }
    }

    private void SpawnChests()
    {
        int chestsToSpawn = Random.Range(minChestsPerDay, maxChestsPerDay + 1);
        List<Transform> availablePoints = new List<Transform>(chestSpawnPoints);

        for (int i = 0; i < chestsToSpawn && availablePoints.Count > 0; i++)
        {
            int index = Random.Range(0, availablePoints.Count);
            Transform point = availablePoints[index];
            availablePoints.RemoveAt(index);

            Vector3 spawnPos = point.position + (Vector3)Random.insideUnitCircle * spawnRadius;
            GameObject chest = Instantiate(chestPrefab, spawnPos, Quaternion.identity, transform);

            spawnedChests.Add(chest);
        }
    }

    private void ClearChests()
    {
        foreach (GameObject chest in spawnedChests)
        {
            if (chest != null)
                Destroy(chest);
        }

        spawnedChests.Clear();
    }
}
