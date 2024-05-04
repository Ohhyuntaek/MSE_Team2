using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SignupInfo : MonoBehaviour
{
    public string ID;
    public string Nickname;
    public string Password;

    public void SetID(string ID){
        this.ID = ID;
    }
    
    public void SetNickname(string Nickname){
        this.Nickname = Nickname;
    }

    public void SetPassword(string Password){
        this.Password = Password;
    }
}
