using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignupSceneManager : MonoBehaviour
{

    [SerializeField] public TMP_Text SignupResultText;

    [SerializeField] public Button SignupButton;



    public void ShowResult(string result, Color color)
    {
        SignupResultText.gameObject.SetActive(true);
        SignupResultText.text = result;
        SignupResultText.color = color;
    }
    
    public void hideResult(){
        SignupResultText.gameObject.SetActive(false);
    }

    public void hideSignupButton(){
        SignupButton.gameObject.SetActive(false);
    }
    
}
