using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginSceneManager : MonoBehaviour
{
    [SerializeField] public TMP_Text LoginResultText;
    [SerializeField] public Button LoginButton;
    
    public void showResult(string result, Color color){
        
        LoginResultText.gameObject.SetActive(true);
        LoginResultText.text = result;
        LoginResultText.color = color;
    }

    public void hideResult(){
        LoginResultText.gameObject.SetActive(false);
    }

    public void hideLoginButton(){
        LoginButton.gameObject.SetActive(false);
    }
}
