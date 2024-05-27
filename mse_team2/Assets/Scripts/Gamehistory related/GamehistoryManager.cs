using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using PlayerServer;

// class for communicating with server related to the game history data
public class GamehistoryManager : MonoBehaviour
{
    
    // urls to communicate with server related to the gamehistory
    private static string BasicURL = "http://localhost:9999/farmwars";

    private string CreateNewURL = BasicURL + "/signup_history";
    private string FindPlayerHistoryURL = BasicURL + "/login_history";
    private string UpdatePlayerHistoryURL = BasicURL + "/update_history";
    private string DeletePlayerHistoryURL = BasicURL + "/delete_history";
    private string TotalRankingHistoryURL = BasicURL + "/ranking_history_total";
    private string EasyRankingHistoryURL = BasicURL + "/ranking_history_easy";
    private string HardRankingHistoryURL = BasicURL + "/ranking_history_hard";

    public GamehistoryManager(){
    
    }

    public void CreateHistory(ParsedPlayer p){
        StopAllCoroutines();
        StartCoroutine(CreateNewRequest(p));
    }
    public void FindPlayerHistory(ParsedPlayer p){
        StopAllCoroutines();
        StartCoroutine(FindPlayerHistoryRequest(p));
    }
    public void UpdatePlayerHistory(GameResultInfo gri){
        StopAllCoroutines();
        StartCoroutine(UpdatePlayerHistoryRequest(gri));
    }
    public void DeletePlayerHistory(ParsedPlayer p){
        StopAllCoroutines();
        StartCoroutine(DeletePlayerHistoryRequest(p));
    }
    public void TotalRankingPlayerHistory(){
        StopAllCoroutines();
        StartCoroutine(TotalRankingHistoryRequest());
    }
    public void EasyRankingPlayerHistory(){
        StopAllCoroutines();
        StartCoroutine(EasyRankingHistoryRequest());
    }
    public void HardRankingPlayerHistory(){
        StopAllCoroutines();
        StartCoroutine(HardRankingHistoryRequest());
    }

    // send create new player gamehistory data request to server part
    IEnumerator CreateNewRequest(ParsedPlayer p)
    {

        string json = JsonUtility.ToJson(p);

        UnityWebRequest request = UnityWebRequest.Post(CreateNewURL, json, "application/json");

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("HTTP Error: " + request.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");
                ParsedGameHistory parsed_gh = JsonUtility.FromJson<ParsedGameHistory>(request.downloadHandler.text);
                
                if (parsed_gh != null){
                    // save new player's gamehistory information in Gamehistory class (maintained during whole app)
                    Gamehistory gh = new Gamehistory(parsed_gh);
                    break;
                }
                break;
        }
    }

    // send find player history request to server part
    IEnumerator FindPlayerHistoryRequest(ParsedPlayer p)
    {
        string json = JsonUtility.ToJson(p);

        UnityWebRequest request = UnityWebRequest.Post(FindPlayerHistoryURL, json, "application/json");

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("HTTP Error: " + request.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");
                ParsedGameHistory parsed_gh = JsonUtility.FromJson<ParsedGameHistory>(request.downloadHandler.text);
                
                if (parsed_gh != null){
                    // save login player's gamehistory information in Gamehistory class (maintained during whole app)
                    Gamehistory gh = new Gamehistory(parsed_gh);
                    break;
                }
                break;
        }
    }

    // update game history (when game ends, save the game result)
    IEnumerator UpdatePlayerHistoryRequest(GameResultInfo gri)
    {
        string json = JsonUtility.ToJson(gri);

        UnityWebRequest request = UnityWebRequest.Post(UpdatePlayerHistoryURL, json, "application/json");

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("HTTP Error: " + request.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");
                
                ParsedGameHistory parsed_gh = JsonUtility.FromJson<ParsedGameHistory>(request.downloadHandler.text);
                
                if (parsed_gh != null){
                    // save updated player's gamehistory information in Gamehistory class (maintained during whole app)
                    Gamehistory gh = new Gamehistory(parsed_gh);
                    break;
                }
                break;
        }
    }

    // delete player's game history data
    IEnumerator DeletePlayerHistoryRequest(ParsedPlayer p)
    {
        string json = JsonUtility.ToJson(p);

        UnityWebRequest request = UnityWebRequest.Post(DeletePlayerHistoryURL, json, "application/json");

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("HTTP Error: " + request.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");
                print(request.downloadHandler.text);
                int result = Int32.Parse(request.downloadHandler.text);
                if (result == 1) { // success to delete
                    // init gamehistory info
                    ParsedGameHistory parsed_gh = new ParsedGameHistory();
                    parsed_gh.privateCode = -1;
                    parsed_gh.easyGame = -1;
                    parsed_gh.easyWin = -1;
                    parsed_gh.hardGame = -1;
                    parsed_gh.hardWin = -1;

                    Gamehistory gh = new Gamehistory(parsed_gh);
                }
                break;
        }
    }

    // send total ranking data request to server part
    IEnumerator TotalRankingHistoryRequest()
    {
        GameLobbySceneManager glsm = FindObjectOfType<GameLobbySceneManager>();

        string json = JsonUtility.ToJson("");

        UnityWebRequest request = UnityWebRequest.Post(TotalRankingHistoryURL, json, "application/json");

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("HTTP Error: " + request.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");
                // get winning rate list
                WinningRateList winningRateList = JsonUtility.FromJson<WinningRateList>("{\"Items\":"+request.downloadHandler.text+"}");
                // show rank list
                glsm.ShowRankResult(winningRateList.Items);
                break;
        }
    }

    // send easy ranking data request to server part
    IEnumerator EasyRankingHistoryRequest()
    {
        GameLobbySceneManager glsm = FindObjectOfType<GameLobbySceneManager>();

        string json = JsonUtility.ToJson("");

        UnityWebRequest request = UnityWebRequest.Post(EasyRankingHistoryURL, json, "application/json");

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("HTTP Error: " + request.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");
                // get winning rate list
                WinningRateList winningRateList = JsonUtility.FromJson<WinningRateList>("{\"Items\":"+request.downloadHandler.text+"}");
                // show rank list
                glsm.ShowRankResult(winningRateList.Items);

                break;
        }
    }

    // send hard ranking data request to server part
    IEnumerator HardRankingHistoryRequest()
    {
        GameLobbySceneManager glsm = FindObjectOfType<GameLobbySceneManager>();
        string json = JsonUtility.ToJson("");

        UnityWebRequest request = UnityWebRequest.Post(HardRankingHistoryURL, json, "application/json");

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("HTTP Error: " + request.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");
                // get winning rate list
                WinningRateList winningRateList = JsonUtility.FromJson<WinningRateList>("{\"Items\":"+request.downloadHandler.text+"}");
                // show rank list
                glsm.ShowRankResult(winningRateList.Items);
                
                break;
        }
    }
}
