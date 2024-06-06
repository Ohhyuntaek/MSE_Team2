using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayerServer;
using UnityEngine.UI;

public class EasyModeGameSceneHandler : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPartInCanvas;
    [SerializeField] private TMP_Text PlayerNickname;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPartInCanvas.GetComponentInChildren<InputField>().text = Player.nickname;
        PlayerPartInCanvas.gameObject.SetActive(false);
        PlayerNickname.text = Player.nickname;
    }

    public string getNickName()
    {
        return Player.nickname;
    }
}
