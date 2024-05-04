using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    public void OpenMainMenuScene(){
        SceneManager.LoadScene("MainMenuScene");
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

    public void QuitApp() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
