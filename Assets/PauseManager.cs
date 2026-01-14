using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    public PlayerInput playerInput;

    public bool IsPaused {  get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;

        playerInput.SwitchCurrentActionMap("UI");
    }
    public void UnpauseGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        playerInput.SwitchCurrentActionMap("Player");
    }
}
