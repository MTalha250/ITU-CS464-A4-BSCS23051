"""Makes every sound and the music for my A4 game, so there's no third-party audio.

Run: python3 Tools/make_audio.py Assets/Audio
Needs numpy. Writes 16-bit mono WAV files at 44.1 kHz.
"""
import sys, os, wave
import numpy as np

RATE = 44100
rng = np.random.default_rng(23051)


def t(seconds):
    return np.arange(int(seconds * RATE)) / RATE


def note(freq, seconds, attack=0.005, decay=6.0, harmonics=(1.0, 0.35, 0.12)):
    """A plucked/bell-ish tone: a few harmonics with a fast attack and an exponential decay."""
    x = t(seconds)
    tone = sum(a * np.sin(2 * np.pi * freq * (k + 1) * x) for k, a in enumerate(harmonics))
    env = np.minimum(x / attack, 1.0) * np.exp(-decay * x)
    return tone * env


def hz(midi):
    return 440.0 * 2 ** ((midi - 69) / 12)


def place(buf, sound, at):
    i = int(at * RATE)
    end = min(len(buf), i + len(sound))
    buf[i:end] += sound[: end - i]


def save(path, x, peak_db):
    x = x / (np.max(np.abs(x)) + 1e-9) * 10 ** (peak_db / 20)
    data = (np.clip(x, -1, 1) * 32767).astype(np.int16)
    with wave.open(path, "wb") as w:
        w.setnchannels(1)
        w.setsampwidth(2)
        w.setframerate(RATE)
        w.writeframes(data.tobytes())
    print(f"{os.path.basename(path)}: {len(x) / RATE:.2f} s, peak {peak_db} dBFS")


def music():
    """16 s loop at 120 bpm: Am - F - C - G pads with a soft arpeggio on top."""
    bar = 2.0
    length = 4 * bar * 2
    buf = np.zeros(int((length + 1) * RATE))          # a second of room for the last tail
    chords = [[57, 60, 64], [53, 57, 60], [48, 52, 55], [55, 59, 62]]   # Am F C G
    for rep in range(2):
        for c, chord in enumerate(chords):
            start = (rep * 4 + c) * bar
            x = t(bar + 0.6)
            env = np.minimum(x / 0.4, 1.0) * np.minimum(1.0, np.maximum(0.0, (bar + 0.6 - x) / 0.6))
            pad = sum(np.sin(2 * np.pi * hz(m) * x) + 0.5 * np.sin(2 * np.pi * hz(m) * 1.003 * x) for m in chord)
            place(buf, 0.18 * pad * env, start)
            place(buf, 0.35 * note(hz(chord[0] - 12), bar, attack=0.01, decay=1.2, harmonics=(1.0, 0.2)), start)
            for k in range(8):                                    # eighth-note arpeggio
                m = chord[k % 3] + 12 * (1 + (k // 3) % 2)
                place(buf, 0.22 * note(hz(m), 0.5, decay=7.0), start + k * bar / 8)
    n = int(length * RATE)
    loop = buf[:n].copy()
    tail = buf[n:]
    loop[: len(tail)] += tail                         # the last chord's tail rings on into the start
    return loop


def coin():
    s = np.zeros(int(0.45 * RATE))
    place(s, note(hz(88), 0.4, decay=9), 0)       # E6
    place(s, note(hz(95), 0.35, decay=8), 0.07)   # B6
    return s


def hit():
    x = t(0.32)
    noise = rng.standard_normal(len(x))
    noise = np.convolve(noise, np.ones(12) / 12, mode="same")      # rough low-pass
    thump = np.sin(2 * np.pi * (140 - 220 * x) * x)
    return (0.9 * noise + 1.2 * thump) * np.exp(-14 * x)


def click():
    x = t(0.06)
    return np.sin(2 * np.pi * 1350 * x) * np.exp(-70 * x) + 0.4 * np.sin(2 * np.pi * 2700 * x) * np.exp(-90 * x)


def win():
    s = np.zeros(int(1.3 * RATE))
    for i, m in enumerate([72, 76, 79, 84]):     # C E G C, rising
        place(s, note(hz(m), 0.9, decay=4), i * 0.12)
    return s


def lose():
    s = np.zeros(int(1.4 * RATE))
    for i, m in enumerate([67, 63, 60, 55]):     # falling, minor
        place(s, note(hz(m), 0.8, decay=3, harmonics=(1.0, 0.5, 0.25, 0.12)), i * 0.18)
    return s


def hum():
    """1 s loop: 110 Hz buzz with a 4 Hz wobble. Whole numbers of cycles, so it loops cleanly."""
    x = t(1.0)
    saw = sum(np.sin(2 * np.pi * 110 * k * x) / k for k in range(1, 7))
    return saw * (0.7 + 0.3 * np.sin(2 * np.pi * 4 * x))


def chime():
    """2 s loop: one soft bell strike that has died away before the loop comes round."""
    s = np.zeros(int(2.0 * RATE))
    place(s, note(hz(81), 1.9, attack=0.003, decay=3.0, harmonics=(1.0, 0.0, 0.3, 0.0, 0.15)), 0)
    place(s, 0.5 * note(hz(88), 1.6, attack=0.003, decay=3.5), 0.12)
    fade = np.ones(len(s))
    fade[-2000:] = np.linspace(1, 0, 2000)
    return s * fade


def main(out):
    os.makedirs(out, exist_ok=True)
    save(os.path.join(out, "Music.wav"), music(), -9)      # music mixed quieter than the effects
    save(os.path.join(out, "Coin.wav"), coin(), -3)
    save(os.path.join(out, "Hit.wav"), hit(), -2)
    save(os.path.join(out, "Click.wav"), click(), -6)
    save(os.path.join(out, "Win.wav"), win(), -3)
    save(os.path.join(out, "Lose.wav"), lose(), -3)
    save(os.path.join(out, "GuardHum.wav"), hum(), -4)
    save(os.path.join(out, "GoalChime.wav"), chime(), -4)


main(sys.argv[1] if len(sys.argv) > 1 else "Assets/Audio")
