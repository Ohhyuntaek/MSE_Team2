using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLobbySceneManager : MonoBehaviour
{
    [SerializeField] public TMP_Text PlayerNickname;
    [SerializeField] public TMP_Text DetailedPlayerID;
    [SerializeField] public TMP_Text DetailedPlayerNickname;
    [SerializeField] public TMP_Text UpdateResultText;
    [SerializeField] private GameObject editAccountInfos;
    void Start()
    {
        PlayerNickname.text = Player.nickname;
        DetailedPlayerID.text = Player.id;
        DetailedPlayerNickname.text = Player.nickname;
    }

    public void showResult(string result, Color color){
        
        UpdateResultText.gameObject.SetActive(true);
        UpdateResultText.text = result;
        UpdateResultText.color = color;
    }

    public void hideResult(){
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

}
