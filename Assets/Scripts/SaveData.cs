using UnityEngine;

// Everything the game remembers between sessions, in PlayerPrefs (Lecture 10).
// Every key is spelled here and nowhere else, and every read has a default.
public static class SaveData
{
    const string UnlockedKey = "unlocked";
    static string BestKey(int level) => "best" + level;

    // Level 1 is always open: the default for a key that was never set.
    public static int Unlocked => PlayerPrefs.GetInt(UnlockedKey, 1);

    public static void Unlock(int level)
    {
        if (level <= Unlocked) return;
        PlayerPrefs.SetInt(UnlockedKey, level);
        PlayerPrefs.Save();
    }

    public static bool HasBest(int level) => PlayerPrefs.HasKey(BestKey(level));
    public static float BestTime(int level) => PlayerPrefs.GetFloat(BestKey(level), 0f);

    // True if this time beat the saved one (or there wasn't one).
    public static bool Record(int level, float seconds)
    {
        if (HasBest(level) && seconds >= BestTime(level)) return false;
        PlayerPrefs.SetFloat(BestKey(level), seconds);
        PlayerPrefs.Save();
        return true;
    }

    // Volume sliders: the key is the mixer parameter's name (MusicVolume, SfxVolume). Default 1 = 0 dB.
    public static float Volume(string parameter) => PlayerPrefs.GetFloat(parameter, 1f);

    public static void SetVolume(string parameter, float value)
    {
        PlayerPrefs.SetFloat(parameter, value);
        PlayerPrefs.Save();
    }
}
