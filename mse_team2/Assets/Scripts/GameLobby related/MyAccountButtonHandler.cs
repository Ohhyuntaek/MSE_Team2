using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyAccountButtonHandler : MonoBehaviour
{
    [SerializeField] private Button myAccountButton;
    [SerializeField] private GameObject editAccountInfos;
    [SerializeField] private GameObject detailedAccountInfos;

    private Sprite originalSprite;

    private void Start() {
        myAccountButton.onClick.AddListener(HandleAccounts);    
        originalSprite = myAccountButton.gameObject.GetComponent<Image>().sprite;
    }

    private void HandleAccounts(){
        if (myAccountButton.gameObject.GetComponent<Image>().sprite == originalSprite){
            detailedAccountInfos.gameObject.SetActive(true);
            editAccountInfos.gameObject.SetActive(false);
            myAccountButton.gameObject.GetComponent<Image>().sprite = null;
        }
        else {
            detailedAccountInfos.gameObject.SetActive(false);
            editAccountInfos.gameObject.SetActive(false);
            myAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite;
        }
    }
}
