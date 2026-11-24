using UnityEngine;

// Keeps the music playing from one scene to the next. Every scene has one, so each scene can be
// tested on its own; the first to wake up stays, and every later copy destroys itself.
public class PersistentMusic : MonoBehaviour
{
    static PersistentMusic instance;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
