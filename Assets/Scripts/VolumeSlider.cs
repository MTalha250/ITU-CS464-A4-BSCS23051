using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// A volume slider for one mixer group (Lecture 09). The slider runs 0.0001 to 1; the mixer wants decibels.
// It lives on the Canvas, not on the hidden pause panel, so Start runs when the scene starts.
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] string parameter = "MusicVolume";   // the group's Volume, exposed and renamed
    [SerializeField] Slider slider;                       // Min Value 0.0001, Max Value 1

    void Start()
    {
        slider.SetValueWithoutNotify(SaveData.Volume(parameter));
        slider.onValueChanged.AddListener(Set);
        Set(slider.value);
    }

    void Set(float value)
    {
        mixer.SetFloat(parameter, Mathf.Log10(value) * 20f);   // 1 -> 0 dB, 0.5 -> -6 dB, 0.0001 -> -80 dB
        SaveData.SetVolume(parameter, value);
    }
}
