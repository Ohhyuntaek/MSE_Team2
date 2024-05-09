using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayerServer;

public class GameLobbySceneManager : MonoBehaviour
{
    [SerializeField] public TMP_Text PlayerNickname;
    [SerializeField] public TMP_Text DetailedPlayerID;
    [SerializeField] public TMP_Text DetailedPlayerNickname;
    [SerializeField] public TMP_Text UpdateResultText;
    [SerializeField] private GameObject editAccountInfos;
    [SerializeField] private TMP_Text DeleteAccountResultText;
    void Start()
    {
        PlayerNickname.text = Player.nickname;
        DetailedPlayerID.text = Player.id;
        DetailedPlayerNickname.text = Player.nickname;
    }

    public void showUpdateResult(string result, Color color){
        
        UpdateResultText.gameObject.SetActive(true);
        UpdateResultText.text = result;
        UpdateResultText.color = color;
    }

    public void hideUpdateResult(){
        UpdateResultText.gameObject.SetActive(false);
    }

    public void hideEditAccountInfos(){
        editAccountInfos.gameObject.SetActive(false);
    }

    public void updatePlayerInfo(){
        PlayerNickname.text = Player.nickname;
        DetailedPlayerID.text = Player.id;
        DetailedPlayerNickname.text = Player.nickname;
    }

    public void showDeleteResult(string result, Color color){
        
        DeleteAccountResultText.gameObject.SetActive(true);
        DeleteAccountResultText.text = result;
        DeleteAccountResultText.color = color;
    }

    public void hideDeleteResult(){
        DeleteAccountResultText.gameObject.SetActive(false);
    }
}
