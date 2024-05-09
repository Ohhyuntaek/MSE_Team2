using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection.Emit;

[Serializable]
public class Player
{
    public string ID;
    public string Nickname;
    public string Password;

    public void SetID(string ID)
    {
        this.ID = ID;
    }
    
    public void SetNickname(string Nickname)
    {
        this.Nickname = Nickname;
    }

    public void SetPassword(string Password)
    {
        this.Password = Password;
    }
}
 