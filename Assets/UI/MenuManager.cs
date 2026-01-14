using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    private bool isOpen = false;
    public GameObject pauseMenu;
    public GameObject characterMenu;
    public GameObject gameOverMenu;
    public static MenuManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
        characterMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    public void OpenMenu(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed & !isOpen)
        {
            PauseManager.instance.PauseGame();
            isOpen = true;
            CharacterStatistic();
            pauseMenu.SetActive(true);
            characterMenu.SetActive(true);
        }
    }
    public void CloseMenu(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed & isOpen)
        {
            PauseManager.instance.UnpauseGame();
            isOpen = false;
            pauseMenu.SetActive(false);
            characterMenu.SetActive(false);
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ResumeGame()
    {
        isOpen = false;
        pauseMenu.SetActive(false);
        characterMenu.SetActive(false);
        
        PauseManager.instance.UnpauseGame();
    }
    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        PauseManager.instance.PauseGame();
    }
    public void LoadGameOver()
    {
        PauseManager.instance.UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void CharacterStatistic()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats stats = player.GetComponent<PlayerStats>();
        PlayerCombatEffects combat = player.GetComponent<PlayerCombatEffects>();

        characterMenu.transform.Find("Health").GetComponent<TMP_Text>().text = stats.maxHP.ToString();
        characterMenu.transform.Find("Speed").GetComponent<TMP_Text>().text = stats.moveSpeed.ToString();
        characterMenu.transform.Find("MeleeDamage").GetComponent<TMP_Text>().text = stats.attackDamage.ToString();
        characterMenu.transform.Find("MeleeCooldown").GetComponent<TMP_Text>().text = stats.attackCooldown.ToString();

        ShowEffects();
    }

    [SerializeField] private Transform effectsContainer;
    [SerializeField] private GameObject effectRowPrefab;

    public void ShowEffects()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerCombatEffects combat = player.GetComponent<PlayerCombatEffects>();
        AutoShootEffect autoShoot = player.GetComponent<AutoShootEffect>();
        if (combat == null) return;

        foreach (Transform child in effectsContainer)
            Destroy(child.gameObject);

        if (combat.lifeStealPercent > 0f)
        {
            GameObject row = Instantiate(effectRowPrefab, effectsContainer);
            row.transform.Find("NameText").GetComponent<TMP_Text>().text = "Life Steal";
            row.transform.Find("ValueText").GetComponent<TMP_Text>().text = $"{combat.lifeStealPercent * 100f:0}%";
        }

        if (combat.burnDamagePerSecond > 0f)
        {
            GameObject row = Instantiate(effectRowPrefab, effectsContainer);
            row.transform.Find("NameText").GetComponent<TMP_Text>().text = "Poison Damage";
            row.transform.Find("ValueText").GetComponent<TMP_Text>().text = $"{combat.burnDamagePerSecond}";

            GameObject row2 = Instantiate(effectRowPrefab, effectsContainer);
            row2.transform.Find("NameText").GetComponent<TMP_Text>().text = "Poison Duration";
            row2.transform.Find("ValueText").GetComponent<TMP_Text>().text = $"{combat.burnDuration}";
        }

        if (autoShoot != null)
        {
            GameObject row = Instantiate(effectRowPrefab, effectsContainer);
            row.transform.Find("NameText").GetComponent<TMP_Text>().text = "Attack Range";
            row.transform.Find("ValueText").GetComponent<TMP_Text>().text = $"{autoShoot.range}";

            GameObject row2 = Instantiate(effectRowPrefab, effectsContainer);
            row2.transform.Find("NameText").GetComponent<TMP_Text>().text = "Fire Rate";
            row2.transform.Find("ValueText").GetComponent<TMP_Text>().text = $"{autoShoot.fireRate}";
        }
    }
}
