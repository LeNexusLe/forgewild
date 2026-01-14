using UnityEngine;
using System.Collections;

public class EnemyNest : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int enemiesToSpawn = 3;
    [SerializeField] private float spawnInterval = 1f;

    [Header("Spawn Offset")]
    [SerializeField] private Vector2 spawnOffset = new Vector2(0, -1.5f);

    private bool isDestroyed = false;

    private void Start()
    {
        NestCounter.RegisterNest(this);
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
            return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Vector3 spawnPos = transform.position + (Vector3)spawnOffset;
        spawnPos += (Vector3)Random.insideUnitCircle * 0.5f;

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    public void DestroyNest()
    {
        if (isDestroyed)
            return;

        isDestroyed = true;
        NestCounter.UnregisterNest(this);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (!isDestroyed)
            NestCounter.UnregisterNest(this);
    }
}
