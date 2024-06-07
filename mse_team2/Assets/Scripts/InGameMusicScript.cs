using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameMusicScript : MonoBehaviour
{
    // HT Play In Game music 

    private AudioSource audioSource;

    [SerializeField]
    private TMP_Text muteButtonText;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnClickMuteMusic()
    {
        if (audioSource.mute) 
        { 
            audioSource.mute = false;
            muteButtonText.text = "Mute On";
        } 
        else
        {
            audioSource.mute = true;
            muteButtonText.text = "Mute Off";
        }
    }
}
