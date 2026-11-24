using UnityEngine;

// Listener: plays the hit sound when the player's Health is damaged.
public class HurtSound : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] AudioClip clip;

    void OnEnable() => health.Damaged += OnHit;
    void OnDisable() => health.Damaged -= OnHit;

    void OnHit(int current, int max) => SfxPlayer.Instance.Play(clip);
}
