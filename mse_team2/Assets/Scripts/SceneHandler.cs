using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    public void OpenTitleScene(){
        SceneManager.LoadScene("Title");
    }
    public void OpenSignupScene()
    {
        SceneManager.LoadScene("SignUp");
    }

    public void OpenLoginScene()
    {
        SceneManager.LoadScene("Login");
    }

    public void OpenGameLobbyScene() 
    {
        SceneManager.LoadScene("GameLobby");    
    }

    public void OpenEasyModeGameLobbyScene(){
        SceneManager.LoadScene("EasyMode");
    }

    public void QuitApp() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
