using UnityEngine;

// A sound that lives in the world (Lecture 10): full volume near it, quieter further away.
[RequireComponent(typeof(AudioSource))]
public class WorldSound : MonoBehaviour
{
    [SerializeField] AudioRolloffMode rolloff = AudioRolloffMode.Logarithmic;
    [SerializeField] float minDistance = 1f;     // full volume inside this
    [SerializeField] float maxDistance = 25f;    // Logarithmic stops fading here; Linear is silent here

    void Awake()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.spatialBlend = 1f;                // fully 3D
        source.rolloffMode = rolloff;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
    }
}
