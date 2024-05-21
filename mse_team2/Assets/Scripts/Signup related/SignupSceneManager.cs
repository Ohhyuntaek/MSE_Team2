using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// this class handles ui related things in signup scene 
public class SignupSceneManager : MonoBehaviour
{

    // text that shows signup result
    [SerializeField] public TMP_Text SignupResultText;

    // button to try signup
    [SerializeField] public Button SignupButton;

    // button to go back to the title scene
    [SerializeField] public Button GoBackButton;


    // show signup result
    public void ShowResult(string result, Color color)
    {
        SignupResultText.gameObject.SetActive(true);
        SignupResultText.text = result;
        SignupResultText.color = color;
    }
    
    // hide signup result
    public void hideResult(){
        SignupResultText.gameObject.SetActive(false);
    }

    // hide signup button
    public void hideSignupButton(){
        SignupButton.gameObject.SetActive(false);
    }

    // hide go back button
    public void hideGoBackButton(){
        GoBackButton.gameObject.SetActive(false);
    }
    
}
