using UnityEngine;

// Listener: one burst of sparks on the player when it's hit (Lecture 09).
public class HitSparks : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] ParticleSystem sparks;      // Looping off, no rate, one burst

    void OnEnable() => health.Damaged += Burst;
    void OnDisable() => health.Damaged -= Burst;

    void Burst(int current, int max) => sparks.Play();
}
