using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Lecture 08's pause menu. Esc toggles it. Resume() runs before every load, because timeScale,
// AudioListener.pause and IsPaused all survive a scene load.
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject firstButton;
    [SerializeField] LevelLoader loader;
    public static bool IsPaused { get; private set; }
    InputAction cancel;

    void Awake() => cancel = InputSystem.actions.FindAction("UI/Cancel");   // Esc

    void Update()
    {
        if (!cancel.WasPressedThisFrame()) return;
        if (GameManager.Instance != null && GameManager.Instance.Over) return;   // the end screens have their own buttons
        if (IsPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        IsPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        EventSystem.current.SetSelectedGameObject(firstButton);   // so the arrow keys and Enter work at once
    }

    public void Resume()
    {
        IsPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void Restart()
    {
        Resume();
        loader.Reload();
    }

    public void QuitToMenu()
    {
        Resume();
        loader.LoadMenu();
    }
}
