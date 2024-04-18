using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignupSceneManager : MonoBehaviour
{

    public TMP_Text SignupResultText;

    public Canvas canvas;



    public void ShowSignupResult(string result)
    {
        Debug.Log(result);
        
        if (result.StartsWith("Success"))
        {
            canvas.gameObject.SetActive(false);
            SignupResultText.gameObject.SetActive(true);
            SignupResultText.text = result;

            new WaitForSeconds(2);

            SceneHandler sc = new SceneHandler();
            sc.OpenGameLobbyScene();
        }
        else if (result.StartsWith("Fail"))
        {
            canvas.gameObject.SetActive(false);
            new WaitForSeconds(2);
            
            canvas.gameObject.SetActive(true);
            SignupResultText.gameObject.SetActive(false);
            SignupResultText.text = "";
        }
    }

    
}
