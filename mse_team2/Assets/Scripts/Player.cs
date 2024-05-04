using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection.Emit;

[Serializable]
public class Player
{
    public long privateCode;
    public string playerId;
    public string playerNickname;
    public string playerPassword;

    public Player(){

    }
    
    public void SetPrivateCode(){
        this.privateCode = 72;
    }

    public void SetID(string ID)
    {
        this.playerId = ID;
    }
    
    public void SetNickname(string Nickname)
    {
        this.playerNickname = Nickname;
    }

    public void SetPassword(string Password)
    {
        this.playerPassword = Password;
    }
}
 