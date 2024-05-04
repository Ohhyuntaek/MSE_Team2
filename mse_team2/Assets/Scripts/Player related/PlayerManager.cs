using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEditor.PackageManager.Requests;

public class PlayerManager : MonoBehaviour
{

    private static string BasicURL = "http://localhost:9999/farmwars";
    private string SignupURL = BasicURL + "/signup";
    private string LoginURL = BasicURL + "/login";
    private string UpdateURL = BasicURL + "/update";

    [SerializeField]
    public TMP_InputField IDInput;
    public TMP_InputField NicknameInput;
    public TMP_InputField PasswordInput;
    public TMP_InputField newNicknameInput;
    public TMP_InputField newPasswordInput;


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
                sm.ShowResult("Fail to sign up!\nID is already exist or inputs are too long!", Color.red);
                yield return new WaitForSeconds(3f);
                sm.hideResult();
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");
                ParsedPlayer parsed_p = JsonUtility.FromJson<ParsedPlayer>(request.downloadHandler.text);
                
                if (parsed_p != null){
                    Player p = new Player(parsed_p);

                    SceneHandler sh = new SceneHandler();

                    sm.ShowResult("Success to sign up!", Color.green);
                    sm.hideSignupButton();
                    yield return new WaitForSeconds(3f);
                    sm.hideResult();
                    sh.OpenGameLobbyScene();
                }
                break;
        }
    }

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
                lgm.showResult("Fail to login!\nInvalid account!", Color.red);
                yield return new WaitForSeconds(3f);
                lgm.hideResult();
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");
                ParsedPlayer parsed_p = JsonUtility.FromJson<ParsedPlayer>(request.downloadHandler.text);

                if (parsed_p != null) {
                    Player p = new Player(parsed_p);

                    SceneHandler sh = new SceneHandler();
                    
                    lgm.showResult("Success to login!", Color.green);
                    lgm.hideLoginButton();
                    yield return new WaitForSeconds(3f);
                    lgm.hideResult();
                    sh.OpenGameLobbyScene();
                }
                else { // password is wrong
                    lgm.showResult("Fail to login!\nPassword is invalid!", Color.red);
                    yield return new WaitForSeconds(3f);
                    lgm.hideResult();
                }
                break;
        }
    }

    IEnumerator UpdateRequest()
    {
        GameLobbySceneManager glm = FindObjectOfType<GameLobbySceneManager>();

        string json = getUpdatedInfoFromFields();
        print(json);
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
                glm.showResult("Fail to update!\nInputs are too long!", Color.red);
                yield return new WaitForSeconds(3f);
                glm.hideResult();
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Request success");
                ParsedPlayer parsed_p = JsonUtility.FromJson<ParsedPlayer>(request.downloadHandler.text);

                if (parsed_p != null) {
                    Player p = new Player(parsed_p);

                    SceneHandler sh = new SceneHandler();
                    
                    glm.showResult("Success to update!", Color.green);
                    glm.hideEditAccountInfos();
                    yield return new WaitForSeconds(3f);
                    glm.hideResult();
                    
                    glm.updatePlayerInfo();
                }
                break;
        }
    }
    
    private string getPlayerFromFields()
    {
        SignupInfo signinfo = new SignupInfo();
        signinfo.SetID(IDInput.text);
        signinfo.SetNickname(NicknameInput.text);
        signinfo.SetPassword(PasswordInput.text);
        
        return JsonUtility.ToJson(signinfo);
    }

    private string getLoginInfoFromFields() {

        LoginInfo loginfo = new LoginInfo();
        loginfo.SetID(IDInput.text);
        loginfo.SetPassword(PasswordInput.text);

        return JsonUtility.ToJson(loginfo);
    }

    private string getUpdatedInfoFromFields(){
        ParsedPlayer pp = new ParsedPlayer();
        pp.password = newPasswordInput.text;
        pp.nickname = newNicknameInput.text;
        pp.id = Player.id;
        pp.privateCode = Player.privateCode;
        
        return JsonUtility.ToJson(pp);
    }

    
}
