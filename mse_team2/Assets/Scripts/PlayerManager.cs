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

    [SerializeField]
    public TMP_InputField IDInput;
    public TMP_InputField NicknameInput;
    public TMP_InputField PasswordInput;


    public void SignupPlayer()
    {
        StartCoroutine(SignupRequest());
    }

    public void LoginPlayer()
    {
        StartCoroutine(LoginRequest());
    }

    IEnumerator SignupRequest()
    {

        string json = getPlayerFromFields();

        UnityWebRequest request = UnityWebRequest.Post(SignupURL, json, "application/json");

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
                SignupSceneManager lgm = FindObjectOfType<SignupSceneManager>();
                // SignupSceneManager.ShowSignupResult(request.downloadHandler.text);
                ParsedPlayer parsed_p = JsonUtility.FromJson<ParsedPlayer>(request.downloadHandler.text);
                // print("&&&& "+request.downloadHandler.text);
                // Debug.Log(parsed_p.privateCode.ToString() + "\t" + parsed_p.id
                // + "\t" + parsed_p.nickname+ "\t" + parsed_p.password);
                
                Player p = new Player(parsed_p);
                // Debug.Log(Player.privateCode.ToString() + "\t" + Player.id
                // + "\t" + Player.nickname+ "\t" + Player.password);

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
                // Debug.Log(request.downloadHandler.text);
                ParsedPlayer parsed_p = JsonUtility.FromJson<ParsedPlayer>(request.downloadHandler.text);

                if (parsed_p != null) {
                    Player p = new Player(parsed_p);

                    SceneHandler sh = new SceneHandler();
                    
                    lgm.showResult("Success to login!", Color.green);
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

    
}
