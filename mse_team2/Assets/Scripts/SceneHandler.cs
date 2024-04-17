using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void OpenSignupScene()
    {
        SceneManager.LoadScene("SignUp");
    }

    public void OpenLoginScene()
    {
        SceneManager.LoadScene("Login");
    }
}
