using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// The main menu: Play opens Level Select, Credits loads scene 6.
// Each panel selects a button as it opens, so the arrow keys and Enter work without the mouse.
public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainPanel, levelsPanel;
    [SerializeField] GameObject mainFirst, levelsFirst;
    [SerializeField] LevelLoader loader;
    [SerializeField] int creditsIndex = 6;
    InputAction cancel;

    void Awake() => cancel = InputSystem.actions.FindAction("UI/Cancel");
    void Start() => ShowMain();

    void Update()
    {
        if (cancel.WasPressedThisFrame() && !mainPanel.activeSelf) ShowMain();   // Esc goes back a panel
    }

    public void ShowMain() => Show(mainPanel, mainFirst);
    public void ShowLevels() => Show(levelsPanel, levelsFirst);
    public void Credits() => loader.Load(creditsIndex);

    void Show(GameObject panel, GameObject first)
    {
        mainPanel.SetActive(panel == mainPanel);
        levelsPanel.SetActive(panel == levelsPanel);
        EventSystem.current.SetSelectedGameObject(first);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
