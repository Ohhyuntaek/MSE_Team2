using UnityEngine;
using UnityEngine.UI;

public class UIAudioControl : MonoBehaviour
{
    public Slider backgroundMusicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // Initialize sliders with current volume levels
        if (AudioManager.Instance != null)
        {
            backgroundMusicSlider.value = AudioManager.Instance.backgroundMusicSource.volume;
            sfxSlider.value = AudioManager.Instance.sfxSource.volume;
        }

        // Add listeners to handle volume changes
        backgroundMusicSlider.onValueChanged.AddListener(SetBackgroundMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void SetBackgroundMusicVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBackgroundMusicVolume(volume);
        }
    }

    private void SetSFXVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(volume);
        }
    }
}
