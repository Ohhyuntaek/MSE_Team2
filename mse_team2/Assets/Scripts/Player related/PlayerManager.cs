using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
//using UnityEditor.PackageManager.Requests;
using System;
using PlayerServer;
using UnityEditor.SceneManagement;

// class for communicating with server related to the player data
public class PlayerManager : MonoBehaviour
{

    private static string BasicURL = "http://localhost:9999/farmwars";
    private string SignupURL = BasicURL + "/signup";
    private string LoginURL = BasicURL + "/login";
    private string UpdateURL = BasicURL + "/update";
    private string DeleteURL = BasicURL + "/delete";

    private GamehistoryManager ghm;

    [SerializeField]
    public TMP_InputField IDInput;
    public TMP_InputField NicknameInput;
    public TMP_InputField PasswordInput;
    public TMP_InputField newNicknameInput;
    public TMP_InputField newPasswordInput;

    void Start() {
        ghm = FindObjectOfType<GamehistoryManager>();
    }


    public void SignupPlayer()
    {
        StopAllCoroutines();
        StartCoroutine(SignupRequest());
    }

    public void LoginPlayer()
    {
        StopAllCoroutines();
        StartCoroutine(LoginRequest());
    }

    public void UpdatePlayer()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateRequest());
    }

    public void DeletePlayer()
    {
        StopAllCoroutines();
        StartCoroutine(DeleteRequest());
    }

    // send signup request to server part
    IEnumerator SignupRequest()
    {
        
        SignupSceneManager sm = FindObjectOfType<SignupSceneManager>();

        string json = getPlayerFromFields();
        UnityWebRequest request = UnityWebRequest.Post(SignupURL, json, "application/json");

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:  // id is already exist or inputs are too long
                Debug.Log("HTTP Error: " + request.error);

                // show fail result
                sm.ShowResult("Fail to sign up!\nID is already exist or inputs are too long!", Color.red);
                yield return new WaitForSeconds(3f);
                sm.hideResult();
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");
                ParsedPlayer parsed_p = JsonUtility.FromJson<ParsedPlayer>(request.downloadHandler.text);
                
                if (parsed_p != null){
                    // save new player's information in Player class (maintained during whole app)
                    Player p = new Player(parsed_p);
                    Debug.Log(Player.privateCode);
                    ghm.CreateHistory(parsed_p);

                    SceneHandler sh = new SceneHandler();
                    
                    // show signup result
                    sm.ShowResult("Success to sign up!", Color.green);
                    sm.hideSignupButton();
                    yield return new WaitForSeconds(3f);
                    sm.hideResult();

                    // move to game lobby scene for playing the game
                    sh.OpenGameLobbyScene();
                }
                break;
        }
    }

    // send login request to server part
    IEnumerator LoginRequest()
    {
        LoginSceneManager lgm = FindObjectOfType<LoginSceneManager>();

        string json = getLoginInfoFromFields();
    
        UnityWebRequest request = UnityWebRequest.Post(LoginURL, json, "application/json");

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:  // id is not in the repository
                Debug.Log("HTTP Error: " + request.error);

                // show fail result
                lgm.showResult("Fail to login!\nInvalid account!", Color.red);
                yield return new WaitForSeconds(3f);
                lgm.hideResult();
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");
                ParsedPlayer parsed_p = JsonUtility.FromJson<ParsedPlayer>(request.downloadHandler.text);

                if (parsed_p != null) {

                    // save login player information in Player class (won't be changed during the whole game)
                    Player p = new Player(parsed_p);

                    ghm.FindPlayerHistory(parsed_p);

                    SceneHandler sh = new SceneHandler();
                    
                    // show login result
                    lgm.showResult("Success to login!", Color.green);
                    lgm.hideLoginButton();
                    yield return new WaitForSeconds(3f);
                    lgm.hideResult();

                    // move to game lobby scene for playing the game
                    sh.OpenGameLobbyScene();
                }
                else { // password is wrong
                    // show fail result
                    lgm.showResult("Fail to login!\nPassword is invalid!", Color.red);
                    yield return new WaitForSeconds(3f);
                    lgm.hideResult();
                }
                break;
        }
    }

    // send updating player information request to server part
    IEnumerator UpdateRequest()
    {
        GameLobbySceneManager glm = FindObjectOfType<GameLobbySceneManager>();

        string json = getUpdatedInfoFromFields();

        UnityWebRequest request = UnityWebRequest.Post(UpdateURL, json, "application/json");

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:  // too long inputs
                Debug.Log("HTTP Error: " + request.error);

                // show fail result
                glm.showUpdateResult("Fail to update!\nInputs are too long!", Color.red);
                yield return new WaitForSeconds(3f);
                glm.hideUpdateResult();
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");

                ParsedPlayer parsed_p = JsonUtility.FromJson<ParsedPlayer>(request.downloadHandler.text);

                if (parsed_p != null) {
                    // save updated player information in Player class
                    Player p = new Player(parsed_p);
                    
                    // show success result
                    glm.showUpdateResult("Success to update!\nJust wait for a moment please :)", Color.green);
                    glm.hideEditAccountInfos();
                    yield return new WaitForSeconds(3f);
                    glm.hideUpdateResult();
                    
                    // show updated player result in UI
                    glm.updatePlayerInfo();
                }
                break;
        }
    }

    // send deleting player information request to server part
    IEnumerator DeleteRequest()
    {
        GameLobbySceneManager glm = FindObjectOfType<GameLobbySceneManager>();

        string json = getPlayerInfo();

        UnityWebRequest request = UnityWebRequest.Post(DeleteURL, json, "application/json");

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

                    ParsedPlayer parsed_p_orig = new ParsedPlayer();
                    parsed_p_orig.privateCode = Player.privateCode;
                    parsed_p_orig.id = Player.id;
                    parsed_p_orig.nickname = Player.nickname;
                    parsed_p_orig.password = Player.password;

                    ghm.DeletePlayerHistory(parsed_p_orig);

                    // init player info
                    ParsedPlayer parsed_p = new ParsedPlayer();
                    parsed_p.privateCode = -1;
                    parsed_p.id = null;
                    parsed_p.nickname = null;
                    parsed_p.password = null;

                    Player p = new Player(parsed_p);

                    SceneHandler sh = new SceneHandler();
                    
                    // show success result
                    glm.showDeleteResult("Success to delete!\nJust wait for a moment please :)", Color.green);
                    yield return new WaitForSeconds(3f);
                    glm.hideDeleteResult();
                    
                    // go back to the title scene
                    sh.OpenTitleScene();
                }
                break;
        }
    }
    
    // get player information for signup from input fields
    private string getPlayerFromFields()
    {
        SignupInfo signinfo = new SignupInfo();
        signinfo.SetID(IDInput.text);
        signinfo.SetNickname(NicknameInput.text);
        signinfo.SetPassword(PasswordInput.text);
        
        return JsonUtility.ToJson(signinfo);
    }

    // get player information for login from input fields
    private string getLoginInfoFromFields() {

        LoginInfo loginfo = new LoginInfo();
        loginfo.SetID(IDInput.text);
        loginfo.SetPassword(PasswordInput.text);

        return JsonUtility.ToJson(loginfo);
    }

    // get new player information to update from input fields
    private string getUpdatedInfoFromFields(){
        ParsedPlayer pp = new ParsedPlayer();
        pp.password = newPasswordInput.text;
        pp.nickname = newNicknameInput.text;
        pp.id = Player.id;
        pp.privateCode = Player.privateCode;
        
        return JsonUtility.ToJson(pp);
    }

    // get player information from Player class
    private string getPlayerInfo(){
        ParsedPlayer pp = new ParsedPlayer();
        pp.privateCode = Player.privateCode;
        pp.id = Player.id;
        pp.nickname = Player.nickname;
        pp.password = Player.password;

        return JsonUtility.ToJson(pp);
    }
    
}
