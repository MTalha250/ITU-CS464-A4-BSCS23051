using UnityEngine;
using UnityEngine.UI;

// Lecture 08's health bar: two filled Images, the fill and a paler lag bar behind it that waits 0.4 s,
// then catches up at 0.8 per second. It listens to the player's Health events.
public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fill;             // Image Type: Filled, Horizontal, Origin: Left
    [SerializeField] Image lag;              // the same, behind it, paler
    [SerializeField] Gradient colour;        // red when low, amber at half, blue when full
    [SerializeField] float lagDelay = 0.4f;
    [SerializeField] float lagSpeed = 0.8f;
    Health health;
    float target = 1f;
    float lagStartsAt;

    void Awake() => health = FindAnyObjectByType<Health>();   // one HUD prefab fits every level

    void OnEnable()
    {
        health.Damaged += Set;
        health.Healed += Set;
    }

    void OnDisable()
    {
        health.Damaged -= Set;
        health.Healed -= Set;
    }

    void Start() => Set(health.Max, health.Max);

    public void Set(int current, int max)
    {
        float next = Mathf.Clamp01((float)current / max);   // cast first: two ints divide to 0
        if (next < target) lagStartsAt = Time.unscaledTime + lagDelay;
        else lag.fillAmount = next;          // healing: the lag bar moves up with it
        target = next;
        fill.fillAmount = target;
        fill.color = colour.Evaluate(target);
    }

    void Update()
    {
        if (Time.unscaledTime < lagStartsAt) return;
        lag.fillAmount = Mathf.MoveTowards(lag.fillAmount, target, lagSpeed * Time.unscaledDeltaTime);
    }
}
