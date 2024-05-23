using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLobbyButtonHandler : MonoBehaviour
{   
    // Buttons, gameobjects in the scene to handle button interactions in the scene
    [SerializeField] private Button myAccountButton;
    [SerializeField] private Button editAccountButton;
    [SerializeField] private Button deleteAccountButton;

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button EasyModeButton;
    [SerializeField] private Button HardModeButton;

    [SerializeField] private Button rankingButton;
    [SerializeField] private Button totalRankingButton;
    [SerializeField] private Button easyRankingButton;
    [SerializeField] private Button hardRankingButton;

    [SerializeField] private GameObject editAccountInfos;
    [SerializeField] private GameObject detailedAccountInfos;
    [SerializeField] private GameObject deletingCheckingButtons;
    [SerializeField] private GameObject RankingPanel;

    // sprites of buttons to easily check button clicking / non-clicking
    private Sprite originalSprite_myAccountButton;
    private Sprite originalSprite_editAccountButton;
    private Sprite originalSprite_deleteAccountButton;
    private Sprite originalSprite_rankingButton;
    private Sprite originalSprite_totalRankingButton;
    private Sprite originalSprite_easyRankingButton;
    private Sprite originalSprite_hardRankingButton;
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
        rankingButton.onClick.AddListener(HandleRanking);
        originalSprite_rankingButton = rankingButton.gameObject.GetComponent<Image>().sprite;
        totalRankingButton.onClick.AddListener(HandleTotalRanking);
        originalSprite_totalRankingButton = totalRankingButton.gameObject.GetComponent<Image>().sprite;
        easyRankingButton.onClick.AddListener(HandleEasyRanking);
        originalSprite_easyRankingButton = easyRankingButton.gameObject.GetComponent<Image>().sprite;
        hardRankingButton.onClick.AddListener(HandleHardRanking);
        originalSprite_hardRankingButton = hardRankingButton.gameObject.GetComponent<Image>().sprite;
    }

    // handling account buttons
    private void HandleAccounts(){
        // when button is clicked
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
            rankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_rankingButton;
            totalRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_totalRankingButton;
            easyRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_easyRankingButton;
            hardRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_hardRankingButton;
            RankingPanel.gameObject.SetActive(false);
        }
        // when button is re-clicked
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
            rankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_rankingButton;
            totalRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_totalRankingButton;
            easyRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_easyRankingButton;
            hardRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_hardRankingButton;
            RankingPanel.gameObject.SetActive(false);
        }
    }

    // handling editing buttons
    private void HandleEditing(){
        // when button is clicked
        if (editAccountButton.gameObject.GetComponent<Image>().sprite == originalSprite_editAccountButton){
            editAccountInfos.gameObject.SetActive(true);
            editAccountButton.gameObject.GetComponent<Image>().sprite = null;
            deletingCheckingButtons.SetActive(false);
            deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;
        }
        // when button is re-clicked
        else {
            editAccountInfos.gameObject.SetActive(false);
            editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;
            deletingCheckingButtons.SetActive(false);
            deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;
        }
    }

    // handling mode buttons
    private void HandleModes(){
        // when button is clicked
        if (startGameButton.gameObject.GetComponent<Image>().sprite == originalSprite_startGameButton){
            EasyModeButton.gameObject.SetActive(true);
            HardModeButton.gameObject.SetActive(true);
            startGameButton.gameObject.GetComponent<Image>().sprite = null;
        }
        // when button is re-clicked
        else {
            EasyModeButton.gameObject.SetActive(false);
            HardModeButton.gameObject.SetActive(false);
            startGameButton.gameObject.GetComponent<Image>().sprite = originalSprite_startGameButton;
        }
        
    }

    // handling deleting part
    private void HandleDeleting(){
        // when button is clicked
        if (deleteAccountButton.gameObject.GetComponent<Image>().sprite == originalSprite_deleteAccountButton){
            deletingCheckingButtons.SetActive(true);
            deleteAccountButton.gameObject.GetComponent<Image>().sprite = null;
            editAccountInfos.gameObject.SetActive(false);
            editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;
        }
        // when button is re-clicked
        else {
            deletingCheckingButtons.SetActive(false);
            deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;
            editAccountInfos.gameObject.SetActive(false);
            editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;
        }

    }

    // reseting deleting buttons and related parts
    public void DeletingButtonReset(){
        
        deletingCheckingButtons.SetActive(false);
        deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;
        editAccountInfos.gameObject.SetActive(false);
        editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;

    }

    // reseting updating buttons and related parts
    public void UpdateButtonReset(){
        editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;
        deletingCheckingButtons.SetActive(false);
        deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;

    }

    // handling ranking related part
    public void HandleRanking(){
        // when button is clicked
        if (rankingButton.gameObject.GetComponent<Image>().sprite == originalSprite_rankingButton){
            myAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_myAccountButton;
            detailedAccountInfos.gameObject.SetActive(false);
            editAccountInfos.gameObject.SetActive(false);
            editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;
            EasyModeButton.gameObject.SetActive(false);
            HardModeButton.gameObject.SetActive(false);
            startGameButton.gameObject.GetComponent<Image>().sprite = originalSprite_startGameButton;
            deletingCheckingButtons.SetActive(false);
            deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;
            rankingButton.gameObject.GetComponent<Image>().sprite = null;
            totalRankingButton.gameObject.GetComponent<Image>().sprite = null;
            easyRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_easyRankingButton;
            hardRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_hardRankingButton;
            RankingPanel.gameObject.SetActive(true);
            
            // show total ranking part
            GamehistoryManager ghm = FindAnyObjectByType<GamehistoryManager>();
            ghm.TotalRankingPlayerHistory();
        }
        // when button is re-clicked
        else {
            myAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_myAccountButton;
            detailedAccountInfos.gameObject.SetActive(false);
            editAccountInfos.gameObject.SetActive(false);
            editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_editAccountButton;
            EasyModeButton.gameObject.SetActive(false);
            HardModeButton.gameObject.SetActive(false);
            startGameButton.gameObject.GetComponent<Image>().sprite = originalSprite_startGameButton;
            deletingCheckingButtons.SetActive(false);
            deleteAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite_deleteAccountButton;
            rankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_rankingButton;
            totalRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_totalRankingButton;
            easyRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_easyRankingButton;
            hardRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_hardRankingButton;
            RankingPanel.gameObject.SetActive(false);
        }
    } 

    // handle total ranking button click event
    public void HandleTotalRanking(){
        // when button is clicked
        if (totalRankingButton.gameObject.GetComponent<Image>().sprite == originalSprite_totalRankingButton){
            totalRankingButton.gameObject.GetComponent<Image>().sprite = null;
            easyRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_easyRankingButton;
            hardRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_hardRankingButton;
        }
    }

    // handle easy ranking button click event
    public void HandleEasyRanking(){
        // when button is clicked
        if (easyRankingButton.gameObject.GetComponent<Image>().sprite == originalSprite_easyRankingButton){
            totalRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_totalRankingButton;
            easyRankingButton.gameObject.GetComponent<Image>().sprite = null;
            hardRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_hardRankingButton;
        }
    }

    // handle hard ranking button click event
    public void HandleHardRanking(){
        // when button is clicked
        if (hardRankingButton.gameObject.GetComponent<Image>().sprite == originalSprite_hardRankingButton){
            totalRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_totalRankingButton;
            easyRankingButton.gameObject.GetComponent<Image>().sprite = originalSprite_easyRankingButton;
            hardRankingButton.gameObject.GetComponent<Image>().sprite = null;
        }
    }

}
