using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

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

    /*public void LoginPlayer()
    {
        StartCoroutine(LoginRequest());
    }*/

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
                Debug.Log("Player sign up successfully");
                Debug.Log(request.downloadHandler.text);
                break;
        }
    }

    /*IEnumerator LoginRequest()
    {

    }*/
    
    private string getPlayerFromFields()
    {
        Player p = new Player();
        p.SetID(IDInput.text);
        p.SetNickname(NicknameInput.text);
        p.SetPassword(PasswordInput.text);

        return JsonUtility.ToJson(p);
    }
}
