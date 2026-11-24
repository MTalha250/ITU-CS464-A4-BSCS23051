using UnityEngine;

// Every sound effect goes through here (Lecture 09): a few AudioSources on the SFX group,
// PlayOneShot, and a random pitch of up to 2 semitones each way. It survives loads, so a sound
// isn't cut off by the scene change (or the object) that caused it.
public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer Instance { get; private set; }

    [SerializeField] AudioSource[] sources;      // four, all with Output set to the mixer's SFX group
    [SerializeField] float pitchRange = 2f;      // ± semitones
    [SerializeField] float minVolume = 0.85f;
    int next;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        foreach (var s in sources) s.ignoreListenerPause = true;   // menu clicks still play while paused
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        AudioSource source = sources[next];
        next = (next + 1) % sources.Length;
        source.pitch = Mathf.Pow(2f, Random.Range(-pitchRange, pitchRange) / 12f);
        source.PlayOneShot(clip, volume * Random.Range(minVolume, 1f));
    }
}
