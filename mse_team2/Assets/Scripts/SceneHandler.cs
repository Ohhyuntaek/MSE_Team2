using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// this class handles scene loading
public class SceneHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject backgroundSoundObject;

    [SerializeField]
    private Button muteButton;

    [SerializeField]
    private TMP_Text muteButtonText;

    private bool isMuteSound = false;

    private AudioSource backgroundSource;

    private void Start()
    {
        backgroundSource = backgroundSoundObject.GetComponent<AudioSource>();
        backgroundSource.Play();
    }

    public void OnClickMuteButton()
    {
        if (isMuteSound == false)
        {
            backgroundSource.mute = true;
            muteButtonText.text = "Mute Off";
            isMuteSound = true;
        } else
        {
            backgroundSource.mute = false;
            muteButtonText.text = "Mute On";
            isMuteSound = false;
        }
    }

    // open title scene
    public void OpenTitleScene(){
        SceneManager.LoadScene("Title");
    }

    // open signup scene
    public void OpenSignupScene()
    {
        SceneManager.LoadScene("SignUp");
    }

    // open login scene
    public void OpenLoginScene()
    {
        SceneManager.LoadScene("Login");
    }

    // open gamelobby scene
    public void OpenGameLobbyScene() 
    {
        SceneManager.LoadScene("GameLobby");    
    }

    // open easy mode game lobby scene
    public void OpenEasyModeScene(){
        SceneManager.LoadScene("EasyMode");
    }

    // close app
    public void QuitApp() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
