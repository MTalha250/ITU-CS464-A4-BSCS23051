using UnityEngine;
using UnityEngine.UI;

// Listener: a red flash over the screen when the player is hurt. It's on the HUD prefab, so it
// listens to the static GameEvents rather than to one particular Health.
public class HitFlash : MonoBehaviour
{
    [SerializeField] Image overlay;              // full screen, red, alpha 0, Raycast Target off
    [SerializeField] float alpha = 0.35f;
    [SerializeField] float fadeSpeed = 1.5f;     // alpha per second

    void OnEnable() => GameEvents.PlayerDamaged += Flash;
    void OnDisable() => GameEvents.PlayerDamaged -= Flash;   // without it, a reload leaves a dead subscriber

    void Flash(int current, int max) => SetAlpha(alpha);

    void Update() => SetAlpha(Mathf.MoveTowards(overlay.color.a, 0f, fadeSpeed * Time.unscaledDeltaTime));

    void SetAlpha(float a)
    {
        Color c = overlay.color;
        c.a = a;
        overlay.color = c;
    }
}
