using System;

// Events with no particular sender, so the HUD can hear the player without a reference to it (Lecture 09).
// They're static and outlive every scene, so whatever subscribes must unsubscribe in OnDisable.
public static class GameEvents
{
    public static event Action<int, int> PlayerDamaged;

    public static void RaisePlayerDamaged(int current, int max) => PlayerDamaged?.Invoke(current, max);
}
