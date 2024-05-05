using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLobbyButtonHandler : MonoBehaviour
{
    [SerializeField] private Button myAccountButton;
    [SerializeField] private Button editAccountButton;
    [SerializeField] private Button deleteAccountButton;

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button EasyModeButton;
    [SerializeField] private Button HardModeButton;



    [SerializeField] private GameObject editAccountInfos;
    [SerializeField] private GameObject detailedAccountInfos;
    [SerializeField] private GameObject deletingCheckingButtons;


    private Sprite originalSprite_myAccountButton;
    private Sprite originalSprite_editAccountButton;
    private Sprite originalSprite_deleteAccountButton;

    private Sprite originalSprite_startGameButton;



    private void Start() {
        myAccountButton.onClick.AddListener(HandleAccounts);    
        originalSprite_myAccountButton = myAccountButton.gameObject.GetComponent<Image>().sprite;
        editAccountButton.onClick.AddListener(HandleEditing);
        originalSprite_editAccountButton = editAccountButton.GetComponent<Image>().sprite; 
        startGameButton.onClick.AddListener(HandleModes);    
        originalSprite_startGameButton = startGameButton.gameObject.GetComponent<Image>().sprite;
        deleteAccountButton.onClick.AddListener(HandleDeleting);    
        originalSprite_deleteAccountButton = deleteAccountButton.gameObject.GetComponent<Image>().sprite;
        }

    private void HandleAccounts(){
        if (myAccountButton.gameObject.GetComponent<Image>().sprite == originalSprite_myAccountButton){
            detailedAccountInfos.gameObject.SetActive(true);
            editAccountInfos.gameObject.SetActive(false);
            myAccountButton.gameObject.GetComponent<Image>().sprite = null;
            editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;
            EasyModeButton.gameObject.SetActive(false);
            HardModeButton.gameObject.SetActive(false);
            startGameButton.gameObject.GetComponent<Image>().sprite = originalSprite_startGameButton;
            deletingCheckingButtons.SetActive(false);
            deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;
        }
        else {
            detailedAccountInfos.gameObject.SetActive(false);
            editAccountInfos.gameObject.SetActive(false);
            myAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_myAccountButton;
            editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;
            EasyModeButton.gameObject.SetActive(false);
            HardModeButton.gameObject.SetActive(false);
            startGameButton.gameObject.GetComponent<Image>().sprite = originalSprite_startGameButton;
            deletingCheckingButtons.SetActive(false);
            deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;
        }
    }

    private void HandleEditing(){
        if (editAccountButton.gameObject.GetComponent<Image>().sprite == originalSprite_editAccountButton){
            editAccountInfos.gameObject.SetActive(true);
            editAccountButton.gameObject.GetComponent<Image>().sprite = null;
            deletingCheckingButtons.SetActive(false);
            deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;
        }
        else {
            editAccountInfos.gameObject.SetActive(false);
            editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;
            deletingCheckingButtons.SetActive(false);
            deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;
        }
    }

    private void HandleModes(){
        if (startGameButton.gameObject.GetComponent<Image>().sprite == originalSprite_startGameButton){
            EasyModeButton.gameObject.SetActive(true);
            HardModeButton.gameObject.SetActive(true);
            startGameButton.gameObject.GetComponent<Image>().sprite = null;
        }
        else {
            EasyModeButton.gameObject.SetActive(false);
            HardModeButton.gameObject.SetActive(false);
            startGameButton.gameObject.GetComponent<Image>().sprite = originalSprite_startGameButton;
        }
        
    }

    private void HandleDeleting(){
        if (deleteAccountButton.gameObject.GetComponent<Image>().sprite == originalSprite_deleteAccountButton){
            deletingCheckingButtons.SetActive(true);
            deleteAccountButton.gameObject.GetComponent<Image>().sprite = null;
            editAccountInfos.gameObject.SetActive(false);
            editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;
        }
        else {
            deletingCheckingButtons.SetActive(false);
            deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;
            editAccountInfos.gameObject.SetActive(false);
            editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;
        }

    }

    public void DeletingButtonReset(){
        
        deletingCheckingButtons.SetActive(false);
        deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;
        editAccountInfos.gameObject.SetActive(false);
        editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;

    }

    public void UpdateButtonReset(){
        editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;
        deletingCheckingButtons.SetActive(false);
        deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;

    }
}
