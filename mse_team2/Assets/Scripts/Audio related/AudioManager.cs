using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    [Header("BGM")]
    public AudioClip lobbyBgm;
    public AudioClip inGameBgm;
    public AudioClip winBgm;
    public AudioClip loseBgm;
    [Header("SFX")]
    public AudioClip buttonClickClip;
    public AudioClip buttonHoverClip;
    public AudioClip buttonConfirmClip;
    public AudioClip buttonClickPauseClip;
    public AudioClip buttonClickUnpauseClip;
    public AudioClip unitClickClip;
    public AudioClip mapClickClip;
    public AudioClip movementClip;
    public AudioClip attackClip;
    public AudioClip destroyClip;

    private Dictionary<string, AudioClip> sfxClips;

    private AudioClip currentBgm;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitSFXClips();
            LoadVolumeSettings();
        }
        else
        {
            Destroy(gameObject);
        }

        currentBgm = lobbyBgm;
    }

    private void Start()
    {
        PlayBackgroundMusic(currentBgm);
    }

    private void InitSFXClips()
    {
        sfxClips = new Dictionary<string, AudioClip>
        {
            { "ButtonClick", buttonClickClip },
            { "ButtonHover", buttonHoverClip },
            { "ButtonConfirm", buttonConfirmClip },
            { "ButtonPause", buttonClickPauseClip },
            { "ButtonUnpause", buttonClickUnpauseClip },
            { "UnitClick", unitClickClip },
            { "MapClick", mapClickClip },
            { "Movement", movementClip },
            { "Attack", attackClip },
            { "Destroy", destroyClip }
        };
    }

    public void PlayBackgroundMusic(AudioClip audioClip)
    {
        if (backgroundMusicSource != null && audioClip != null)
        {
            backgroundMusicSource.clip = audioClip;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
    }

    public void PlayBackgroundMusicByName(string name)
    {
        switch (name)
        {
            case "lobbyBgm":
                currentBgm = lobbyBgm;
                break;

            case "inGameBgm":
                currentBgm = inGameBgm;
                break;

            case "winBgm":
                currentBgm = winBgm;
                break;

            case "loseBgm":
                currentBgm = loseBgm;
                break;
        }

        PlayBackgroundMusic(currentBgm);
    }

    public void PlaySFX(string clipName)
    {
        if (sfxClips.ContainsKey(clipName) && sfxSource != null)
        {
            sfxSource.PlayOneShot(sfxClips[clipName]);
        }
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = volume;
        }
        PlayerPrefs.SetFloat("BackgroundMusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolumeSettings()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = PlayerPrefs.GetFloat("BackgroundMusicVolume", 1f);
        }
        if (sfxSource != null)
        {
            sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }
    }
}
