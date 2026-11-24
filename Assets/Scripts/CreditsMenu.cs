using UnityEngine;
using UnityEngine.EventSystems;

// The credits scene: one button, back to the menu through the loader (LoadNext wraps 6 to 0).
public class CreditsMenu : MonoBehaviour
{
    [SerializeField] GameObject first;
    [SerializeField] LevelLoader loader;

    void Start() => EventSystem.current.SetSelectedGameObject(first);
    public void Back() => loader.LoadNext();
}
