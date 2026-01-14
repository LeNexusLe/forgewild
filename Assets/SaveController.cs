using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    [SerializeField] private WorldTime.WorldTime worldTime;

    [SerializeField] private GameObject gameSavedPopup;
    [SerializeField] private WorldTime.WorldBiomeChange biomeChange;
    [SerializeField] private AutoShootEffectSO autoShootEffectSO;
    private void OnEnable()
    {
        if (worldTime != null)
            worldTime.DayChanged += SaveGame;
    }

    private void OnDisable()
    {
        if (worldTime != null)
            worldTime.DayChanged -= SaveGame;
    }

    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        LoadGame();
    }

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats stats = player.GetComponent<PlayerStats>();
        PlayerCombatEffects combat = player.GetComponent<PlayerCombatEffects>();
        AutoShootEffect autoShoot = player.GetComponent<AutoShootEffect>();

        SaveData saveData = new SaveData
        {
            playerPosition = player.transform.position,

            maxHP = stats.maxHP,
            currentHP = stats.currentHP,
            moveSpeed = stats.moveSpeed,
            attackDamage = stats.attackDamage,
            attackCooldown = stats.attackCooldown,

            lifeStealPercent = combat != null ? combat.lifeStealPercent : 0f,
            burnDuration = combat != null ? combat.burnDuration : 0f,
            burnDamagePerSecond = combat != null ? combat.burnDamagePerSecond : 0f,

            hasAutoShoot = autoShoot != null,
            autoShootFireRate = autoShoot != null ? autoShoot.fireRate : 0f,
            autoShootRange = autoShoot != null ? autoShoot.range : 0f,

            currentDay = worldTime.CurrentDay,
            currentBiomeIndex = biomeChange.CurrentBiomeIndex
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData, true));
        ShowGameSavedPopup();
    }

    public void LoadGame()
    {
        if (!File.Exists(saveLocation))
        {
            SaveGame();
            return;
        }

        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats stats = player.GetComponent<PlayerStats>();
        PlayerCombatEffects combat = player.GetComponent<PlayerCombatEffects>();

        player.transform.position = saveData.playerPosition;

        stats.maxHP = saveData.maxHP;
        stats.currentHP = saveData.currentHP;
        stats.moveSpeed = saveData.moveSpeed;
        stats.attackDamage = saveData.attackDamage;
        stats.attackCooldown = saveData.attackCooldown;

        if (combat != null)
        {
            combat.lifeStealPercent = saveData.lifeStealPercent;
            combat.burnDuration = saveData.burnDuration;
            combat.burnDamagePerSecond = saveData.burnDamagePerSecond;
        }

        if (saveData.hasAutoShoot)
        {
            autoShootEffectSO.Apply(player);

            AutoShootEffect autoShoot = player.GetComponent<AutoShootEffect>();
            autoShoot.fireRate = saveData.autoShootFireRate;
            autoShoot.range = saveData.autoShootRange;
        }

        worldTime.SetDay(saveData.currentDay);
        biomeChange.SetBiome(saveData.currentBiomeIndex);
    }

    private void ShowGameSavedPopup()
    {
        if (gameSavedPopup == null) return;

        gameSavedPopup.SetActive(true);
        Animator animator = gameSavedPopup.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("GameSavedAnimation", 0);
        }
    }
}
