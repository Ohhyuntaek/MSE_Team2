using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// this class handles scene loading
public class SceneHandler : MonoBehaviour
{

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
    public void OpenEasyModeGameLobbyScene(){
        SceneManager.LoadScene("EasyModeGameLobby");
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
