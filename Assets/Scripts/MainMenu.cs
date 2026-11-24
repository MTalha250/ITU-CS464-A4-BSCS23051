using UnityEngine;
using UnityEngine.EventSystems;

// The main menu: Play starts Level 1, Credits loads scene 6. Play is selected, so Enter works at once.
public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject first;
    [SerializeField] LevelLoader loader;
    [SerializeField] int creditsIndex = 6;

    void Start() => EventSystem.current.SetSelectedGameObject(first);

    public void Play() => loader.Load(1);
    public void Credits() => loader.Load(creditsIndex);

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
