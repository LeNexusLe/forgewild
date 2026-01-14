using UnityEngine;
using WorldTime;

public class BossManager : MonoBehaviour
{
    [SerializeField] private WorldTime.WorldTime worldTime;
    [SerializeField] private WorldBiomeChange biomeManager;

    [Header("Bosses in order")]
    [SerializeField] private GameObject[] bossPrefabs;

    [Header("Chest")]
    [SerializeField] private GameObject chestPrefab;

    [SerializeField] private Transform bossSpawnPoint;

    private GameObject activeBoss;

    public void SpawnBoss()
    {
        int currentBossIndex = biomeManager.CurrentBiomeIndex;

        if (currentBossIndex >= bossPrefabs.Length) return;

        worldTime.StopTime();

        activeBoss = Instantiate(bossPrefabs[currentBossIndex], bossSpawnPoint.position, Quaternion.identity);

        IBoss boss = activeBoss.GetComponent<IBoss>();
        boss.OnBossKilled += OnBossKilled;
    }

    private void OnBossKilled()
    {
        IBoss boss = activeBoss.GetComponent<IBoss>();
        boss.OnBossKilled -= OnBossKilled;

        Vector3 spawnPos = activeBoss.transform.position;

        Destroy(activeBoss);

        Instantiate(chestPrefab, spawnPos, Quaternion.identity);

        worldTime.StartTime();
        SetTimeToMorning();
    }

    private void SetTimeToMorning()
    {
        worldTime.SetExactTime(6, 0);
    }
}
