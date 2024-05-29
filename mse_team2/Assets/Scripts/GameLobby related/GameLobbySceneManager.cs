using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayerServer;
using Unity.VisualScripting;
using System;

public class GameLobbySceneManager : MonoBehaviour
{
    // player account related part
    [SerializeField] public TMP_Text PlayerNickname;
    [SerializeField] public TMP_Text DetailedPlayerID;
    [SerializeField] public TMP_Text DetailedPlayerNickname;
    [SerializeField] public TMP_Text UpdateResultText;
    [SerializeField] private GameObject editAccountInfos;
    [SerializeField] private TMP_Text DeleteAccountResultText;

    // ranking related part
    [SerializeField] private TMP_Text Player_ranking_nickname;
    [SerializeField] private TMP_Text Player_ranking_totalGame;
    [SerializeField] private TMP_Text Player_ranking_totalWin;
    [SerializeField] private TMP_Text Player_ranking_totalLose;
    [SerializeField] private TMP_Text Player_ranking_totalWinningRate;
    [SerializeField] private TMP_Text Player_ranking_easyGame;
    [SerializeField] private TMP_Text Player_ranking_easyWin;
    [SerializeField] private TMP_Text Player_ranking_easyLose;
    [SerializeField] private TMP_Text Player_ranking_easyWinningRate;
    [SerializeField] private TMP_Text Player_ranking_hardGame;
    [SerializeField] private TMP_Text Player_ranking_hardWin;
    [SerializeField] private TMP_Text Player_ranking_hardLose;
    [SerializeField] private TMP_Text Player_ranking_hardWinningRate;
    [SerializeField] private List<TMP_Text> Rank_Nicknames;
    [SerializeField] private List<TMP_Text> Rank_WinningRates;
    [SerializeField] private TMP_Text PlayerRank;

    void Start()
    {
        // set basic information text about player account
        PlayerNickname.text = Player.nickname;
        DetailedPlayerID.text = Player.id;
        DetailedPlayerNickname.text = Player.nickname;
        
        // set basic information text about player's ranking history
        Player_ranking_nickname.text = Player.nickname;
        
        Player_ranking_totalGame.text = (Gamehistory.easyGame + Gamehistory.hardGame).ToString();
        Player_ranking_totalWin.text = (Gamehistory.easyWin + Gamehistory.hardWin).ToString();
        Player_ranking_totalLose.text = (Gamehistory.easyGame + Gamehistory.hardGame - Gamehistory.easyWin - Gamehistory.hardWin).ToString();
        if ((Gamehistory.easyGame + Gamehistory.hardGame)==0){
            Player_ranking_totalWinningRate.text = 0.ToString()+"%";
        }
        else {
            Player_ranking_totalWinningRate.text = (Math.Truncate((float)((Gamehistory.easyWin + Gamehistory.hardWin)*100/(Gamehistory.easyGame + Gamehistory.hardGame)))).ToString()+"%";
        }

        Player_ranking_easyGame.text = Gamehistory.easyGame.ToString();
        Player_ranking_easyWin.text = Gamehistory.easyWin.ToString();
        Player_ranking_easyLose.text = (Gamehistory.easyGame - Gamehistory.easyWin).ToString();
        if (Gamehistory.easyGame==0){
            Player_ranking_easyWinningRate.text = 0.ToString()+"%";
        }
        else {
            Player_ranking_easyWinningRate.text = (Math.Truncate((float)(Gamehistory.easyWin*100/Gamehistory.easyGame))).ToString()+"%";
        }

        Player_ranking_hardGame.text = Gamehistory.hardGame.ToString();
        Player_ranking_hardWin.text = Gamehistory.hardWin.ToString();
        Player_ranking_hardLose.text = (Gamehistory.hardGame - Gamehistory.hardWin).ToString();
        if (Gamehistory.hardGame==0){
            Player_ranking_hardWinningRate.text = 0.ToString();
        }
        else {
            Player_ranking_hardWinningRate.text= (Math.Truncate((float)(Gamehistory.hardWin*100/Gamehistory.hardGame))).ToString()+"%";
        }
        
    }

    // show updating request result
    public void showUpdateResult(string result, Color color){
        
        UpdateResultText.gameObject.SetActive(true);
        UpdateResultText.text = result;
        UpdateResultText.color = color;
    }

    // hide updating request result
    public void hideUpdateResult(){
        UpdateResultText.gameObject.SetActive(false);
    }

    // hide edit account part game objects
    public void hideEditAccountInfos(){
        editAccountInfos.gameObject.SetActive(false);
    }

    // update player information with new values
    public void updatePlayerInfo(){
        // update player information in account part
        PlayerNickname.text = Player.nickname;
        DetailedPlayerID.text = Player.id;
        DetailedPlayerNickname.text = Player.nickname;

        // update player information in history part
        Player_ranking_nickname.text = Player.nickname;
    }

    // show deleting request result
    public void showDeleteResult(string result, Color color){
        
        DeleteAccountResultText.gameObject.SetActive(true);
        DeleteAccountResultText.text = result;
        DeleteAccountResultText.color = color;
    }

    // hide deleting request result
    public void hideDeleteResult(){
        DeleteAccountResultText.gameObject.SetActive(false);
    }

    // ranking of the modes
    public void ShowRankResult(List<WinningRate> wrl){
        for (int i = 0; i < wrl.Count; i++){
            if (i<5) {
                // print top 5 player's information
                Rank_Nicknames[i].text = wrl[i].nickname;
                Rank_WinningRates[i].text = wrl[i].winningRate.ToString()+"%";
            }
            // print current player's rank
            if (wrl[i].nickname.Equals(Player.nickname)){
                PlayerRank.text = (i+1).ToString();
            }
        }

    }
}
