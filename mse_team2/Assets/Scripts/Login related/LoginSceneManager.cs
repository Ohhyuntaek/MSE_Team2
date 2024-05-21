using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// handles ui related things in login scene
public class LoginSceneManager : MonoBehaviour
{
    // text part to show login result
    [SerializeField] public TMP_Text LoginResultText;
    
    // button to try login
    [SerializeField] public Button LoginButton;

    // button to go back to the title scene
    [SerializeField] public Button GoBackButton;
    
    // show login result
    public void showResult(string result, Color color){
        
        LoginResultText.gameObject.SetActive(true);
        LoginResultText.text = result;
        LoginResultText.color = color;
    }

    // hide login result
    public void hideResult(){
        LoginResultText.gameObject.SetActive(false);
    }

    // hide login button
    public void hideLoginButton(){
        LoginButton.gameObject.SetActive(false);
    }

    // hide go back button
    public void hideGoBackButton(){
        GoBackButton.gameObject.SetActive(false);
    }
}
