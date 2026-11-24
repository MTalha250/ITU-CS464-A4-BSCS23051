using UnityEngine;
using UnityEngine.UI;

// Gives every button on this canvas (hidden panels too) the menu click.
public class UiSounds : MonoBehaviour
{
    [SerializeField] AudioClip click;

    void Awake()
    {
        foreach (Button b in GetComponentsInChildren<Button>(true))
            b.onClick.AddListener(() => SfxPlayer.Instance.Play(click, 0.7f));
    }
}
